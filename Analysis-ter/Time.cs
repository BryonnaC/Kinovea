using System;

namespace Analysistem.Utils
{
    public enum Unit : int // delay units
    {
        Seconds = 0,
        Milliseconds = 1,
        Microseconds = 1_000,
        Nanoseconds = 1_000_000,
    }

    public static class Time
    {
        public static double ToUnits(this TimeSpan elapsed, Unit unit)
        {
            return elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond * (int)unit;
        }

        public static double ToFromUnits(this double time, Unit oldUnits, Unit newUnits)
        {
            double secScale = newUnits == Unit.Seconds ? 1000 : 1;
            newUnits = newUnits == Unit.Seconds ? Unit.Milliseconds : newUnits;
            switch(oldUnits)
            {
                case Unit.Seconds: return time * (double) newUnits * 1000;
                case Unit.Milliseconds: return time * (double) newUnits / secScale;
                case Unit.Microseconds: return oldUnits < newUnits ? time / 1_000 : time * 1_000 * secScale;
                case Unit.Nanoseconds: return newUnits == Unit.Microseconds ? time / 1_000 : time / (1_000_000 * secScale);
                default: return time; // this will never be reached
            }
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}
