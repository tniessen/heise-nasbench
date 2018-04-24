
#include "log.h"

#include <Windows.h>

TCHAR * GetLogTagName(LOGTAG tag) {
#define V(name) case TAG_##name: return _T(#name)
    switch (tag) {
		V(GENERATE_INIT_DONE);
		V(GENERATE_FILE_DONE);
		V(COPY_FILE_START);
		V(COPY_FILE_STREAM);
		V(COPY_FILE_CHUNK);
		V(COPY_FILE_END);
		V(COPY_GROUP_START);
		V(COPY_GROUP_END);
		V(VERIFY_INIT_DONE);
		V(VERIFY_OKAY);
		V(VERIFY_FAIL);
    default:
        return NULL;
    }
#undef V
}

LOGENTRY benchlog[LOG_MAX_SIZE];
unsigned int logSize;
unsigned int logOffset;

TIMER logTimer;
CRITICAL_SECTION logCrit;

void LogInit() {
    logSize = 0;
    logOffset = 0;

    InitializeCriticalSectionAndSpinCount(&logCrit, 4000);

    InitTimer(&logTimer);
    StartTimer(&logTimer);
}

BOOL LogPush(LOGTAG tag, PLOGDATA data) {
    BOOL success;
    unsigned int index;
    PLOGENTRY entry;

    EnterCriticalSection(&logCrit);

    if (LogFull()) {
        SetLastError(ERROR_OUTOFMEMORY);
        success = FALSE;
    }
    else {
        index = (logOffset + logSize++) % LOG_MAX_SIZE;

        entry = benchlog + index;
        entry->tag = tag;
        entry->timestamp = GetTimerValue(&logTimer);
        entry->data = data;

        success = TRUE;
    }

    LeaveCriticalSection(&logCrit);

    return success;
}

BOOL LogFull() {
    BOOL full;

    EnterCriticalSection(&logCrit);
    full = (logSize == LOG_MAX_SIZE);
    LeaveCriticalSection(&logCrit);

    return full;
}

BOOL LogPop(LOGTAG *tag, uint64_t *timestamp, PLOGDATA *data) {
    BOOL success;
    unsigned int index;
    PLOGENTRY entry;

    EnterCriticalSection(&logCrit);

    if (logSize == 0) {
        success = FALSE;
    }
    else {
        index = (logOffset++) % LOG_MAX_SIZE;
        logSize--;

        entry = benchlog + index;
        *tag = entry->tag;
        *timestamp = entry->timestamp;
        *data = entry->data;

        success = TRUE;
    }

    LeaveCriticalSection(&logCrit);

    return success;
}
