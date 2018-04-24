/* @file timer.h */

#pragma once

#include <stdint.h>

typedef struct {
    LARGE_INTEGER start;
    LARGE_INTEGER frequency;
} TIMER, *PTIMER;

void InitTimer(PTIMER timer);

void StartTimer(PTIMER timer);

uint64_t GetAndRestartTimer(PTIMER timer);

uint64_t GetTimerValue(PTIMER timer);
