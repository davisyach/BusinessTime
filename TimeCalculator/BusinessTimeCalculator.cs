using System;
using System.Collections.Generic;

namespace TimeCalculator
{
    public class Time : IComparable<Time>
    {
        //test for branch
        private int _hour = 0;
        public int Hour
        {
            get { return _hour; }
            set
            {
                if(value > 23)
                {
                    throw new ArgumentException("Hours must be between 0-23");
                }
                else
                {
                    _hour = value;
                }
            }
        }

        private int _minute = 0;
        public int Minute
        {
            get { return _minute; }
            set
            {
                if (value > 59)
                {
                    throw new ArgumentException("Minutes must be between 0-59");
                }
                else
                {
                    _minute = value;
                }
            }
        }

        private int _second = 0;
        public int Second
        {
            get { return _second; }
            set
            {
                if (value > 59)
                {
                    throw new ArgumentException("Seconds must be between 0-59");
                }
                else
                {
                    _second = value;
                }
            }
        }

        public Time()
        {

        }

        public Time(int hour)
        {
            Hour = hour;
            Minute = 0;
            Second = 0;
        }

        public Time(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
            Second = 0;
        }
        public Time(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public static Time Now
        {
            get { return new Time { Hour = DateTime.Now.Hour, Minute = DateTime.Now.Minute, Second = DateTime.Now.Second }; }
        }

        public override string ToString()
        {
            return $"{_hour}:{_minute}:{_second}";
        }

        public int CompareTo(Time other)
        {
            if (other == null) throw new ArgumentException("Must provide non-null object to compare.");

            if (other.Hour == _hour && other.Minute == _minute && other.Second == _second) return 0;

            return other.Hour > _hour && other.Minute > _minute && other.Second > _second ? -1 : 1;
        }
    }

    public static class BusinessTimeCalculator
    {
        private static HashSet<DateTime> _holidays = new HashSet<DateTime>();
        //change to test source control
        public static bool IsOpenWeekends = false;
        public static bool IsOpenHolidays = false;
        public static bool IsAlwaysOpen = false;
        public static bool IsOpen24Hours = false;

        /// <summary>
        /// When true, converts the current timezone to the supplied state's timezone to calculate business hours in that timezone,
        /// then converts back to the current timezone.
        /// </summary>
        public static bool ConvertTimeZones = false;

        /// <summary>
        /// Determines whether the calculation is exact. When true, it calculates using all of the requested business hours.
        /// When false, it determines if the returned date should simply be the start of the next business day.
        /// </summary>
        public static bool FullBusinessHours = false;
        
        public static Time BusinessHoursStart;
        public static Time BusinessHoursEnd;

        public static bool AddHoliday(DateTime holiday)
        {
            try
            {
                if (_holidays == null) _holidays = new HashSet<DateTime>();
                
                _holidays.Add(holiday);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddHoliday(IEnumerable<DateTime> holidays)
        {
            try
            {
                if (_holidays == null) _holidays = new HashSet<DateTime>();

                _holidays.UnionWith(holidays);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClearHolidays()
        {
            _holidays.Clear();
        }

        public static bool RemoveHoliday(DateTime holiday)
        {
            try
            {
                return _holidays.RemoveWhere(d => d.Date.Equals(holiday.Date)) > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsHoliday(DateTime dateTime)
        {
            return _holidays.Contains(dateTime);
        }

        public static bool IsWeekend(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWithinBusinessHours(DateTime dateTime)
        {
            return (dateTime.Hour > BusinessHoursStart.Hour && 
               dateTime.Minute > BusinessHoursStart.Minute &&
               dateTime.Second > BusinessHoursStart.Second) &&
               (dateTime.Hour < BusinessHoursEnd.Hour && 
               dateTime.Minute < BusinessHoursEnd.Minute && 
               dateTime.Second < BusinessHoursEnd.Second);
        }

        public static bool IsOpen(DateTime dateTime)
        {
            return IsAlwaysOpen || 
                ((IsOpenHolidays || !IsHoliday(dateTime)) && 
                (IsOpenWeekends || !IsWeekend(dateTime)) && 
                (IsOpen24Hours || IsWithinBusinessHours(dateTime)));
        }
    }
}
