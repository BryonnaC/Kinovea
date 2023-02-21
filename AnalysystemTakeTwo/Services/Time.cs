using System;
using static System.Math;

namespace AnalysystemTakeTwo
{
    public enum Unit : int // delay units
    {
        Seconds = 0, // atm, only to be used with ToFromUnits() and NOTHING ELSE AT ALL EVER
        Milliseconds = 3,
        Microseconds = 6,
        Nanoseconds = 9,
    }

    public class Milliseconds
    {
        double amount;

        public Milliseconds(double amount)
        {
            this.amount = amount;
        }

        public static implicit operator Milliseconds(double amount) => new Milliseconds(amount);
        public static implicit operator double(Milliseconds value) => value.amount;
    }

    public static class Time
    {
        public static double ToUnits(this TimeSpan elapsed, Unit unit)
        {
            int scale = (int)Pow(10, (int)unit);

            return elapsed.Ticks / (double)TimeSpan.TicksPerSecond * scale;
        }

        public static double ToFromUnits(this double time, Unit toUnits, Unit fromUnits)
        {
            int scale = (int)Pow(10, Abs(fromUnits - toUnits));

            switch (toUnits)
            {
                case Unit.Seconds: return time * scale;
                case Unit.Milliseconds: 
                case Unit.Microseconds: return toUnits < fromUnits ? time * scale : time / scale;
                case Unit.Nanoseconds: return time / scale;
                default: return time; // this will never be reached
            }
        }

        public static double ToSeconds(this Milliseconds milliseconds)
        {
            return ToFromUnits(milliseconds, Unit.Seconds, Unit.Milliseconds);
        }

        public static string GetTimestamp(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}
