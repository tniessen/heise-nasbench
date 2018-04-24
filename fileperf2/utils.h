/** @file utils.h */

#pragma once

#include <tchar.h>
#include <Windows.h>

TCHAR* FormatLastErrorMessage();

BOOL AllocInitRandom(size_t size, void **out, uint64_t seed);
