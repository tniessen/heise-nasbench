/* @file log.h */

#pragma once

#include <stdint.h>
#include <Windows.h>
#include <tchar.h>

#include "timer.h"

typedef enum {
    TAG_GENERATE_INIT_DONE,
    TAG_GENERATE_FILE_DONE,
    TAG_COPY_FILE_START,
    TAG_COPY_FILE_STREAM,
    TAG_COPY_FILE_CHUNK,
    TAG_COPY_FILE_END,
    TAG_COPY_GROUP_START,
    TAG_COPY_GROUP_END,
    TAG_VERIFY_INIT_DONE,
    TAG_VERIFY_OKAY,
    TAG_VERIFY_FAIL
} LOGTAG;

TCHAR * GetLogTagName(LOGTAG tag);

typedef union {
    struct {
        unsigned int groupIndex;
        unsigned int fileIndex;
        uint64_t fileSize;
        TCHAR path[MAX_PATH];
    } generateFile;
    struct {
        unsigned int groupIndex;
        unsigned int fileCount;
        uint64_t fileSize;
    } copyGroup;
    struct {
        unsigned int groupIndex;
        unsigned int fileIndex;
        uint64_t fileSize;
        TCHAR sourcePath[MAX_PATH];
        TCHAR targetPath[MAX_PATH];
        uint64_t bytesTransferred;
        uint64_t timeSinceStart;
    } copyFile;
    struct {
        unsigned int groupIndex;
        unsigned int fileIndex;
        uint64_t fileSize;
        TCHAR path[MAX_PATH];
        TCHAR *failMessage;
    } verify;
} LOGDATA, *PLOGDATA;

typedef struct {
    LOGTAG tag;
    uint64_t timestamp;
    PLOGDATA data;
} LOGENTRY, *PLOGENTRY;

#define LOG_MAX_SIZE (64 * 1024)

void LogInit();

BOOL LogFull();

BOOL LogPush(LOGTAG tag, PLOGDATA data);

BOOL LogPop(LOGTAG *tag, uint64_t *timestamp, PLOGDATA *data);
