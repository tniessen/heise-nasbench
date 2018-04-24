
#include <Windows.h>

#include "fast_random.h"

// This LCG is based on a formula taken from a library developed at the University of California,
// see http://nuclear.llnl.gov/CNP/rng/rngman/index.html.
static uint64_t rand64(uint64_t seed) {
    return (2862933555777941757L * seed) + 3037000493;
}

BOOL InitializeRandomData(void *data, size_t size, register uint64_t seed) {
    if (size % 8 != 0) {
        SetLastError(ERROR_INVALID_PARAMETER);
        return FALSE;
    }

    // Using 64 bit vectors with the 64 bit LCG reduces the CPU time
    // to a fraction of the time the original implementation needed.
    uint64_t *data64 = (uint64_t *)data;

    for (size_t i = 0, b = size / 8; i < b; i++) {
        data64[i] = seed = rand64(seed);
    }

    return TRUE;
}
