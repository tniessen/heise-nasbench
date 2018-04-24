/** @file fast_random.h */

#pragma once

#include <stdint.h>

/**
* Initializes a memory buffer with pseudo-random contents using an optimized
* 64 bit LCG and the given seed. If the buffer size is not a multiple of 64
* bits, this function will fail and `GetLastError()` will return
* `ERROR_INVALID_PARAMETER`.
*
* @return 0 on failure, a non-zero integer on success
*/
int InitializeRandomData(void *data, size_t size, uint64_t seed);
