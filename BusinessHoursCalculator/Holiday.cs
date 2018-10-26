using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTime
{
    public class Holiday
    {
        public DateTime Date { get; set; }
        public bool IsOpen { get; set; }
        public Time HolidayHourStart { get; set; }
        public Time HolidayHoursEnd { get; set; }
    }
}
