using System;
using System.Collections.Generic;
using System.Text;

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
                if (value > 23)
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
}
