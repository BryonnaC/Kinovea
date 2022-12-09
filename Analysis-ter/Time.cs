using System;

namespace Analysistem.Utils
{
    public enum Unit : int // delay units
    {
        Seconds = 0, // atm, only to be used with ToFromUnits() and NOTHING ELSE AT ALL EVER
        Milliseconds = 3,
        Microseconds = 6,
        Nanoseconds = 9,
    }

    public static class Time
    {
        public static double ToUnits(this TimeSpan elapsed, Unit unit)
        {
            int scale = (int)Math.Pow(10, (int)unit);

            return elapsed.Ticks / (double)TimeSpan.TicksPerSecond * scale;
        }

        public static double ToFromUnits(this double time, Unit oldUnits, Unit newUnits)
        {
            int scale = (int)Math.Pow(10, Math.Abs(newUnits - oldUnits));

            switch (oldUnits)
            {
                case Unit.Seconds: return time * scale;
                case Unit.Milliseconds: 
                case Unit.Microseconds: return oldUnits > newUnits ? time / scale : time * scale;
                case Unit.Nanoseconds: return time / scale;
                default: return time; // this will never be reached
            }
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}
