using System;

namespace Analysistem.Utils
{
    public enum Units : int // delay units
    {
        Milliseconds = 1,
        Microseconds = 1_000,
        Nanoseconds = 1_000_000,
    }

    public static class Time
    {
        public static double ToUnits(this TimeSpan elapsed, Units unit)
        {
            return elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond * (int)unit;
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}
