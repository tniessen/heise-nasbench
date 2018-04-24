
#include <Windows.h>

#include "timer.h"

#define GET_TIME(timer, now) (now.QuadPart - timer->start.QuadPart) * 1000000 / timer->frequency.QuadPart

void InitTimer(PTIMER timer) {
    QueryPerformanceFrequency(&timer->frequency);
}

void StartTimer(PTIMER timer) {
    QueryPerformanceCounter(&timer->start);
}

uint64_t GetAndRestartTimer(PTIMER timer) {
    LARGE_INTEGER now;
    QueryPerformanceCounter(&now);
    uint64_t time = GET_TIME(timer, now);
    timer->start.QuadPart = now.QuadPart;
    return time;
}

uint64_t GetTimerValue(PTIMER timer) {
    LARGE_INTEGER now;
    QueryPerformanceCounter(&now);
    return GET_TIME(timer, now);
}
