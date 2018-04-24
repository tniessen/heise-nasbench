
#include <Windows.h>

#include "fast_random.h"

TCHAR* FormatLastErrorMessage() {
    TCHAR *result;
    int success;

    success = FormatMessage(
        FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_IGNORE_INSERTS,
        NULL,
        GetLastError(),
        MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
        (TCHAR *)&result,
        0,
        NULL
    );

    if (!success) {
        return NULL;
    }

    return result;
}

BOOL AllocInitRandom(size_t size, void **out, uint64_t seed) {
    if (size % 8 != 0) {
        size += 8 - (size % 8);
    }

    *out = malloc(size);
    if (!*out) {
        SetLastError(ERROR_OUTOFMEMORY);
        return FALSE;
    }

    return InitializeRandomData(*out, size, seed);
}
