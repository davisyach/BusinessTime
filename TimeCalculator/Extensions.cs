using System;
using System.Collections.Generic;
using System.Text;

namespace TimeCalculator
{
    public static class Extensions
    {
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
