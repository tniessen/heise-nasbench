
#include <Windows.h>
#include <tchar.h>
#include <stdint.h>
#include <assert.h>
#include <stdio.h>

#include "fast_random.h"
#include "utils.h"
#include "log.h"

typedef struct {
    uint64_t size;
    unsigned int count;
} FILEGROUP, *PFILEGROUP;

BOOL ParseSize(TCHAR *str, uint64_t *out) {
    TCHAR *end;

    *out = _tcstoui64(str, &end, 10);

    if (end == str)
        return 0;

    switch (tolower(*end)) {
    case 'g':
        *out *= 1024;
    case 'm':
        *out *= 1024;
    case 'k':
        *out *= 1024;
        break;
    case '\0':
        break;
    default:
        return FALSE;
    }

    return TRUE;
}

BOOL ParseFileGroup(TCHAR *str, unsigned int *count, uint64_t *size) {
    TCHAR *sizestr = _tcschr(str, 'x');
    if (sizestr == NULL)
        return FALSE;

    if (!ParseSize(sizestr + 1, size))
        return FALSE;

    // Yes, this will overflow if a group contains more than 256^sizeof(int)-1 files. The
    // obvious solution is to use multiple groups.
    TCHAR *countEnd;
    *count = (unsigned int)_tcstoui64(str, &countEnd, 10);
    if (countEnd != sizestr)
        return FALSE;

    return TRUE;
}

void GetTestFilePath(TCHAR *out, size_t outSize, TCHAR *dir, unsigned int group, unsigned int file, BOOL onePerGroup) {
    if (onePerGroup) file = 0;
    _stprintf_s(out, outSize, _T("%s\\%02d-%04d"), dir, group, file);
}

BOOL DeleteTestFiles(TCHAR *dir, unsigned int groupCount, FILEGROUP *groups, BOOL onePerGroup) {
    TCHAR path[MAX_PATH];
    DWORD attr;

    for (unsigned int g = 0; g < groupCount; g++) {
        for (unsigned int f = 0; f < groups[g].count && (f == 0 || !onePerGroup); f++) {
            GetTestFilePath(path, MAX_PATH, dir, g, f, onePerGroup);
            attr = GetFileAttributes(path);
            if (attr != INVALID_FILE_ATTRIBUTES) {
                if (!DeleteFile(path)) {
                    return FALSE;
                }
            }
        }
    }

    return TRUE;
}

BOOL GenerateTestFiles(TCHAR *dir, unsigned int groupCount, FILEGROUP *groups, BOOL onePerGroup) {
    size_t maxsize = 0;
    for (unsigned int i = 0; i < groupCount; i++) {
        if (groups[i].size > maxsize)
            maxsize = groups[i].size;
    }

    void *data;
    if (!AllocInitRandom(maxsize, &data, 0))
        return FALSE;

    LogPush(TAG_GENERATE_INIT_DONE, NULL);
    PLOGDATA logData;

    TCHAR path[MAX_PATH];
    DWORD written;
    for (unsigned int g = 0; g < groupCount; g++) {
        for (unsigned int f = 0; f < groups[g].count && (f == 0 || !onePerGroup); f++) {
            GetTestFilePath(path, MAX_PATH, dir, g, f, onePerGroup);
            HANDLE hFile = CreateFile(path, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
            if (hFile == INVALID_HANDLE_VALUE)
                return FALSE;

            // TODO: Do not attempt to create files >4GB using a single call to WriteFile (size is limited to a DWORD)
            if (!WriteFile(hFile, data, groups[g].size, &written, NULL)) {
                int error = GetLastError();
                CloseHandle(hFile);
                SetLastError(error);
                return FALSE;
            }

            FlushFileBuffers(hFile);
            CloseHandle(hFile);

            logData = malloc(sizeof(LOGDATA));
            if (!logData) {
                SetLastError(ERROR_OUTOFMEMORY);
                return FALSE;
            }
            logData->generateFile.groupIndex              = g;
            logData->generateFile.fileIndex               = f;
            logData->generateFile.fileSize                = groups[g].size;
            _tcscpy_s(logData->generateFile.path, MAX_PATH, path);

            LogPush(TAG_GENERATE_FILE_DONE, logData);
        }
    }

    return TRUE;
}

typedef struct {
    PTIMER       timer;
    unsigned int groupIndex;
    unsigned int fileIndex;
    uint64_t     fileSize;
    TCHAR        *sourcePath;
    TCHAR        *targetPath;
    uint64_t     bytesTransferred;
} FILE_COPY_INFO, *PFILE_COPY_INFO;

static PLOGDATA FileCopyInfoToLogData(PFILE_COPY_INFO info) {
    PLOGDATA data = malloc(sizeof(LOGDATA));
    if (!data) return NULL;

    data->copyFile.groupIndex                    = info->groupIndex;
    data->copyFile.fileIndex                     = info->fileIndex;
    data->copyFile.fileSize                      = info->fileSize;
    _tcscpy_s(data->copyFile.sourcePath, MAX_PATH, info->sourcePath);
    _tcscpy_s(data->copyFile.targetPath, MAX_PATH, info->targetPath);
    data->copyFile.bytesTransferred              = info->bytesTransferred;
    data->copyFile.timeSinceStart                = GetTimerValue(info->timer);

    return data;
}

static PLOGDATA GroupToLogData(unsigned int groupIndex, PFILEGROUP group) {
    PLOGDATA data = malloc(sizeof(LOGDATA));
    if (!data) return NULL;

    data->copyGroup.groupIndex = groupIndex;
    data->copyGroup.fileCount  = group->count;
    data->copyGroup.fileSize   = group->size;

    return data;
}

DWORD CALLBACK CopyProgressCallback(
    _In_     LARGE_INTEGER TotalFileSize,
    _In_     LARGE_INTEGER TotalBytesTransferred,
    _In_     LARGE_INTEGER StreamSize,
    _In_     LARGE_INTEGER StreamBytesTransferred,
    _In_     DWORD         dwStreamNumber,
    _In_     DWORD         dwCallbackReason,
    _In_     HANDLE        hSourceFile,
    _In_     HANDLE        hDestinationFile,
    _In_opt_ LPVOID        lpData
) {
    PFILE_COPY_INFO info = (PFILE_COPY_INFO)lpData;
    info->bytesTransferred = TotalBytesTransferred.QuadPart;

    PLOGDATA logData = FileCopyInfoToLogData(info);

    if (dwCallbackReason == CALLBACK_STREAM_SWITCH) {
        LogPush(TAG_COPY_FILE_STREAM, logData);
    }
    else if(dwCallbackReason == CALLBACK_CHUNK_FINISHED) {
        LogPush(TAG_COPY_FILE_CHUNK, logData);
    }
    else {
        assert(0);
    }

    return PROGRESS_CONTINUE;
}

BOOL CopyFiles(TCHAR *sourceDir, TCHAR *targetDir, unsigned int groupCount, FILEGROUP *groups, BOOL oneSourcePerGroup, BOOL oneTargetPerGroup, BOOL allowSystemBuffer) {
    TCHAR sourcePath[MAX_PATH], targetPath[MAX_PATH];

    TIMER timer;
    InitTimer(&timer);

    FILE_COPY_INFO info;
    info.sourcePath = sourcePath;
    info.targetPath = targetPath;
    info.timer = &timer;

    PFILEGROUP group;

    for (info.groupIndex = 0; info.groupIndex < groupCount; info.groupIndex++) {
        group = groups + info.groupIndex;

        LogPush(TAG_COPY_GROUP_START, GroupToLogData(info.groupIndex, group));

        for (info.fileIndex = 0; info.fileIndex < group->count; info.fileIndex++) {
            info.fileSize = group->size;
            info.bytesTransferred = 0;

            GetTestFilePath(sourcePath, MAX_PATH, sourceDir, info.groupIndex, info.fileIndex, oneSourcePerGroup);
            GetTestFilePath(targetPath, MAX_PATH, targetDir, info.groupIndex, info.fileIndex, oneTargetPerGroup);

            int flags = 0;
            if (!allowSystemBuffer) flags |= COPY_FILE_NO_BUFFERING;

            StartTimer(&timer);

            LogPush(TAG_COPY_FILE_START, FileCopyInfoToLogData(&info));

            if (!CopyFileEx(sourcePath, targetPath, CopyProgressCallback, &info, NULL, flags)) {
                return FALSE;
            }

            LogPush(TAG_COPY_FILE_END, FileCopyInfoToLogData(&info));
        }

        LogPush(TAG_COPY_GROUP_END, GroupToLogData(info.groupIndex, group));
    }

    return TRUE;
}

PLOGDATA CreateVerifyLogData(unsigned int groupIndex, unsigned int fileIndex, uint64_t fileSize, TCHAR *path) {
    PLOGDATA data = malloc(sizeof(LOGDATA));
    if (!data) return NULL;

    data->verify.groupIndex    = groupIndex;
    data->verify.fileIndex     = fileIndex;
    data->verify.fileSize      = fileSize;
    _tcscpy_s(data->verify.path, MAX_PATH, path);

    return data;
}

BOOL VerifyFiles(TCHAR *dir, unsigned int groupCount, FILEGROUP *groups, BOOL onePerGroup) {
    size_t maxsize = 0;
    for (unsigned int i = 0; i < groupCount; i++) {
        if (groups[i].size > maxsize)
            maxsize = groups[i].size;
    }

    void *data;
    if (!AllocInitRandom(maxsize, &data, 0))
        return FALSE;

    DWORD blockSize = 32 * 1024;

    void *actualData = malloc(blockSize);
    if (!actualData) {
        SetLastError(ERROR_OUTOFMEMORY);
        free(data);
        return FALSE;
    }

    LogPush(TAG_VERIFY_INIT_DONE, NULL);

    PLOGDATA logData;

    TCHAR path[MAX_PATH];
    LARGE_INTEGER actualFileSize;
    for (unsigned int g = 0; g < groupCount; g++) {
        for (unsigned int f = 0; f < groups[g].count && (f == 0 || !onePerGroup); f++) {
            GetTestFilePath(path, MAX_PATH, dir, g, f, onePerGroup);

            HANDLE hFile = CreateFile(path, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

            if (hFile == INVALID_HANDLE_VALUE) {
                TCHAR *err = FormatLastErrorMessage();
                logData = CreateVerifyLogData(g, f, groups[g].size, path);
                logData->verify.failMessage = calloc(1024, sizeof(TCHAR));
                _stprintf_s(logData->verify.failMessage, 1024, _T("CreateFile() failed: %s"), err);
                LogPush(TAG_VERIFY_FAIL, logData);
                continue;
            }

            if (GetFileSizeEx(hFile, &actualFileSize) && actualFileSize.QuadPart != groups[g].size) {
                logData = CreateVerifyLogData(g, f, groups[g].size, path);
                logData->verify.failMessage = calloc(200, sizeof(TCHAR));
                if (!logData->verify.failMessage) {
                    SetLastError(ERROR_OUTOFMEMORY);
                    return FALSE;
                }
                _stprintf_s(logData->verify.failMessage, 200, _T("File size %lld differs from %llu"), actualFileSize.QuadPart, groups[g].size);
                LogPush(TAG_VERIFY_FAIL, logData);
				continue;
            }

            BOOL okay = TRUE;

            DWORD nRead;
            uint64_t nReadTotal = 0;

            while (nReadTotal < groups[g].size) {
                // Safe cast: the minimum is not greater than blockSize which is a DWORD
                DWORD nRequest = (DWORD) min(groups[g].size - nReadTotal, blockSize);

                if (!ReadFile(hFile, actualData, nRequest, &nRead, NULL)) {
                    TCHAR *err = FormatLastErrorMessage();
                    logData = CreateVerifyLogData(g, f, groups[g].size, path);
                    logData->verify.failMessage = calloc(1024, sizeof(TCHAR));
                    _stprintf_s(logData->verify.failMessage, 1024, _T("ReadFile() failed: %s"), err);
                    LogPush(TAG_VERIFY_FAIL, logData);
                    okay = FALSE;
                    break;
                }

                if (nRead != nRequest) {
                    logData = CreateVerifyLogData(g, f, groups[g].size, path);
                    logData->verify.failMessage = calloc(200, sizeof(TCHAR));
                    _stprintf_s(logData->verify.failMessage, 200, _T("Expected %lu bytes, but read %lu"), nRequest, nRead);
                    LogPush(TAG_VERIFY_FAIL, logData);
                    okay = FALSE;
					break;
                }

                if (memcmp((uint8_t *)data + nReadTotal, actualData, nRead) != 0) {
                    logData = CreateVerifyLogData(g, f, groups[g].size, path);
                    logData->verify.failMessage = _T("Data differs");
                    LogPush(TAG_VERIFY_FAIL, logData);
                    okay = FALSE;
                    break;
                }

                nReadTotal += nRead;
            }

            CloseHandle(hFile);

            if (okay) {
                logData = CreateVerifyLogData(g, f, groups[g].size, path);
                LogPush(TAG_VERIFY_OKAY, logData);
            }
        }
    }

    free(data);
    free(actualData);

    return TRUE;
}

static void WritePendingLogEntries() {
    LOGTAG tag;
    uint64_t timestamp;
    PLOGDATA data;

    while (LogPop(&tag, &timestamp, &data)) {
        switch (tag) {
        case TAG_GENERATE_INIT_DONE:
            _tprintf(
                _T("%10llu\t%s\n"),
                timestamp, GetLogTagName(tag)
            );
            break;
        case TAG_GENERATE_FILE_DONE:
            _tprintf(
                _T("%10llu\t%s\t%u\t%u\t%llu\t%s\n"),
                timestamp, GetLogTagName(tag),
                data->generateFile.groupIndex, data->generateFile.fileIndex, data->generateFile.fileSize,
                data->generateFile.path
            );
            break;
        case TAG_COPY_FILE_START:
        case TAG_COPY_FILE_STREAM:
        case TAG_COPY_FILE_CHUNK:
        case TAG_COPY_FILE_END:
            _tprintf(
                _T("%10llu\t%s\t%u\t%u\t%llu\t%s\t%s\t%llu\t%llu\n"),
                timestamp, GetLogTagName(tag),
                data->copyFile.groupIndex, data->copyFile.fileIndex, data->copyFile.fileSize,
                data->copyFile.sourcePath, data->copyFile.targetPath,
                data->copyFile.bytesTransferred, data->copyFile.timeSinceStart
            );
            break;
        case TAG_COPY_GROUP_START:
        case TAG_COPY_GROUP_END:
            _tprintf(
                _T("%10llu\t%s\t%u\t%u\t%llu\n"),
                timestamp, GetLogTagName(tag),
                data->copyGroup.groupIndex, data->copyGroup.fileCount, data->copyGroup.fileSize
            );
            break;
        case TAG_VERIFY_INIT_DONE:
            _tprintf(
                _T("%10llu\t%s\n"),
                timestamp, GetLogTagName(tag)
            );
            break;
        case TAG_VERIFY_OKAY:
            _tprintf(
                _T("%10llu\t%s\t%u\t%u\t%llu\t%s\n"),
                timestamp, GetLogTagName(tag),
                data->verify.groupIndex, data->verify.fileIndex, data->verify.fileSize,
                data->verify.path
            );
            break;
        case TAG_VERIFY_FAIL:
            _tprintf(
                _T("%10llu\t%s\t%u\t%u\t%llu\t%s\t%s\n"),
                timestamp, GetLogTagName(tag),
                data->verify.groupIndex, data->verify.fileIndex, data->verify.fileSize,
                data->verify.path, data->verify.failMessage // TODO: Trim the error message
            );
            break;
        }

        if (data != NULL) free(data);
    }

    fflush(stdout);
}

DWORD WINAPI WriteLogThread(LPVOID lpParam) {
    PBOOL shutdown = (PBOOL)lpParam;

    while (!*shutdown) {
        WritePendingLogEntries();
        Sleep(10);
    }

    WritePendingLogEntries();

    return 0;
}

typedef enum {
    COMMAND_GENERATE,
    COMMAND_COPY,
    COMMAND_VERIFY
} COMMAND;

void ShowUsage() {
    _ftprintf(stderr,
        _T("Usage:\n")
        _T("fileperf2 generate [--single-target-per-group] [--delete-target] <DIRECTORY> <GROUP...>\n")
        _T("fileperf2 copy [--single-source-per-group] [--single-target-per-group] [--delete-target] [--system-buffer] <SOURCE> <TARGET> <GROUP...>\n")
        _T("fileperf2 verify [--single-target-per-group] <DIRECTORY> <GROUP...>\n")
    );
}

int _tmain(int argc, TCHAR* argv[]) {
    COMMAND cmd;
    TCHAR *firstDir = NULL, *secondDir = NULL;
    BOOL deleteTarget = FALSE, singleSourcePerGroup = FALSE, singleTargetPerGroup = FALSE;
    BOOL allowSystemBuffer = FALSE;

    unsigned int count;
    uint64_t size;
    FILEGROUP *groups = NULL;
    unsigned int groupCount = 0;

    if (argc < 2) {
        ShowUsage();
        return 1;
    }

    if (_tcsicmp(argv[1], _T("generate")) == 0) {
        cmd = COMMAND_GENERATE;
    }
    else if (_tcsicmp(argv[1], _T("copy")) == 0) {
        cmd = COMMAND_COPY;
    }
    else if (_tcsicmp(argv[1], _T("verify")) == 0) {
        cmd = COMMAND_VERIFY;
    }
    else {
        ShowUsage();
        return 1;
    }

    for (int i = 2; i < argc; i++) {
        if (_tcsicmp(argv[i], _T("--single-source-per-group")) == 0) {
            if (cmd != COMMAND_COPY) {
                _ftprintf(stderr, _T("--single-source-per-group can only be used with copy\n"));
                return 1;
            }
            singleSourcePerGroup = TRUE;
        }
        else if (_tcsicmp(argv[i], _T("--single-target-per-group")) == 0) {
            singleTargetPerGroup = TRUE;
        }
        else if (_tcsicmp(argv[i], _T("--delete-target")) == 0) {
            if (cmd != COMMAND_COPY && cmd != COMMAND_GENERATE) {
                _ftprintf(stderr, _T("--delete-target can only be used with copy or generate\n"));
                return 1;
            }
            deleteTarget = TRUE;
        }
        else if (_tcsicmp(argv[i], _T("--system-buffer")) == 0) {
            if (cmd != COMMAND_COPY) {
                _ftprintf(stderr, _T("--system-buffer can only be used with copy\n"));
                return 1;
            }
            allowSystemBuffer = TRUE;
        }
        else if (_tcsicmp(argv[i], _T("--help")) == 0) {
            ShowUsage();
            return 0;
        }
        else if((cmd == COMMAND_GENERATE || cmd == COMMAND_COPY || cmd == COMMAND_VERIFY) && firstDir == NULL) {
            firstDir = argv[i];
        }
        else if (cmd == COMMAND_COPY && secondDir == NULL) {
            secondDir = argv[i];
        }
        else if (ParseFileGroup(argv[i], &count, &size)) {
            FILEGROUP *oldGroups = groups;
            groups = (FILEGROUP *)calloc(groupCount + 1, sizeof(FILEGROUP));
            if (groups == NULL) {
                _ftprintf(stderr, _T("Failed to allocate memory for option parsing.\r\n"));
                return 1;
            }

            if (oldGroups != NULL) {
                memcpy(groups, oldGroups, sizeof(FILEGROUP) * groupCount);
                free(oldGroups);
            }

            FILEGROUP *fg = groups + groupCount++;
            fg->count = count;
            fg->size = size;
        }
        else {
            _ftprintf(stderr, _T("Unexpected argument: %s\n"), argv[i]);
            return 1;
        }
    }

    if (firstDir == NULL) {
        _ftprintf(stderr, _T("Directory path required.\n"));
        return 1;
    }

    if (cmd == COMMAND_COPY && secondDir == NULL) {
        _ftprintf(stderr, _T("Target path required.\n"));
        return 1;
    }

    if (groupCount == 0) {
        _ftprintf(stderr, _T("At least one file group must be specified.\n"));
        return 1;
    }

    SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);
    SetThreadPriority(GetCurrentThread(), THREAD_PRIORITY_HIGHEST);

    LogInit();

    BOOL shutdown = FALSE;

    HANDLE hLogThread = CreateThread(NULL, 0, WriteLogThread, &shutdown, 0, NULL);
    if (hLogThread == NULL) {
        _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
        return 2;
    }

    SetThreadPriority(hLogThread, THREAD_PRIORITY_NORMAL);

    switch (cmd) {
    case COMMAND_GENERATE:
        if (deleteTarget) {
            if (!DeleteTestFiles(firstDir, groupCount, groups, singleTargetPerGroup)) {
                _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
                return 2;
            }
        }
        if (!GenerateTestFiles(firstDir, groupCount, groups, singleTargetPerGroup)) {
            _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
            return 2;
        }
        break;
    case COMMAND_COPY:
        if (deleteTarget) {
            if (!DeleteTestFiles(secondDir, groupCount, groups, singleTargetPerGroup)) {
                _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
                return 2;
            }
        }
        if (!CopyFiles(firstDir, secondDir, groupCount, groups, singleSourcePerGroup, singleTargetPerGroup, allowSystemBuffer)) {
            _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
            return 2;
        }
        break;
    case COMMAND_VERIFY:
        if (!VerifyFiles(firstDir, groupCount, groups, singleTargetPerGroup)) {
            _ftprintf(stderr, _T("Error %d: %s\n"), GetLastError(), FormatLastErrorMessage());
            return 2;
        }
        break;
    }

    shutdown = TRUE;

    WaitForSingleObject(hLogThread, INFINITE);

    return 0;
}
