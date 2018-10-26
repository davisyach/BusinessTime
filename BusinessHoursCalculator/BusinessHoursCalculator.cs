using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessTime.Calculator
{
    /// <summary>
    /// Provides functions to perform business hour calculations.
    /// </summary>
    public static class BusinessHoursCalculator
    {
        /// <summary>
        /// Collection of holidays.
        /// </summary>
        private static HashSet<Holiday> _holidays = new HashSet<Holiday>();

        /// <summary>
        /// Flag for if the business is open on weekends.
        /// </summary>
        public static bool IsOpenWeekends { get; set; } = false;
        /// <summary>
        /// Flag for if the business is open on holidays.
        /// </summary>
        public static bool IsOpenHolidays { get; set; } = false;
        /// <summary>
        /// Flag for if the business is always open.
        /// </summary>
        public static bool IsAlwaysOpen { get; set; } = false;
        /// <summary>
        /// Flag for if the business is open 24 hours a day.
        /// </summary>
        public static bool IsOpen24Hours { get; set; } = false;
        /// <summary>
        /// When true, converts the current timezone to the supplied state's timezone to calculate business hours in that timezone,
        /// then converts back to the current timezone.
        /// </summary>
        public static bool ConvertTimeZones { get; set; } = false;
        /// <summary>
        /// Determines whether the calculation is exact. When true, it calculates using all of the requested business hours.
        /// When false, it determines if the returned date should simply be the start of the next business day.
        /// </summary>
        public static bool FullBusinessHours { get; set; } = false;
        /// <summary>
        /// The business open time.
        /// </summary>
        public static Time BusinessHoursStart { get; set; }
        /// <summary>
        /// The business close time.
        /// </summary>
        public static Time BusinessHoursEnd { get; set; }

        /// <summary>
        /// Adds business hours to the given start date, taking into consideration IsOpenWeekends, IsOpenHolidays, IsOpen24Hours, and IsAlwaysOpen.
        /// Also converts timezone if ConvertTimeZones is set and city and state and/or zipcode are provided.
        /// </summary>
        /// <param name="startDate">The start date for the calculation.</param>
        /// <param name="hoursToAdd">The number of hours to add.</param>
        /// <param name="city">The city to convert the business time to.</param>
        /// <param name="state">The state to convert the business time to.</param>
        /// <param name="zipCode">The zipcode to convert the business time to.</param>
        /// <returns>A new DateTime object with the calculated date.</returns>
        public static DateTime AddBusinessHours(DateTime startDate, double hoursToAdd, string city = "", string state = "", string zipCode = "")
        {
            TimeZoneInfo destTimeZone = TimeZoneConverter.GetTimeZoneInfo(city, state, zipCode);

            if(ConvertTimeZones && destTimeZone != null)
                startDate = TimeZoneInfo.ConvertTime(startDate, TimeZoneInfo.Local, destTimeZone);

            var calcDate = startDate;
            var minutesToAdd = hoursToAdd * 60;

            while(minutesToAdd > 0)
            {
                if (IsOpen(calcDate))
                {
                    minutesToAdd--;
                }

                calcDate = calcDate.AddMinutes(1);
            }
            
            if (ConvertTimeZones && destTimeZone != null)
                calcDate = TimeZoneInfo.ConvertTime(calcDate, destTimeZone, TimeZoneInfo.Local);

            return calcDate;
        }

        /// <summary>
        /// Gets the total number of business hours between two dates.
        /// </summary>
        /// <param name="startDate">The start date for the range.</param>
        /// <param name="endDate">The end date for the range.</param>
        /// <returns>The total number of business hours as a double.</returns>
        public static double GetTotalBusinessHours(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(1, (endDate - startDate).Minutes).Where(h =>
                    IsOpen(startDate.AddMinutes(h))
            ).Count() / 60.0;
        }

        /// <summary>
        /// Adds a holiday to the collection of holidays.
        /// </summary>
        /// <param name="holiday">The holiday to add.</param>
        /// <returns>True if the holiday was successfully added.</returns>
        public static bool AddHoliday(DateTime holiday)
        {
            try
            {                
                return _holidays.Add(new Holiday() { Date = holiday });
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a collection of holidays to the collection of holidays.
        /// </summary>
        /// <param name="holidays">A collection of holidays.</param>
        /// <returns>True if the holidays were successfully added.</returns>
        public static bool AddHoliday(IEnumerable<DateTime> holidays)
        {
            try
            {
                _holidays.UnionWith(holidays.Select(h => new Holiday() { Date = h }));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears the collection of holidays.
        /// </summary>
        public static void ClearHolidays()
        {
            _holidays.Clear();
        }

        /// <summary>
        /// Removes the specified holiday from the collection of holidays.
        /// </summary>
        /// <param name="holiday">The holiday to remove.</param>
        /// <returns>True if any holidays were removed.</returns>
        public static bool RemoveHoliday(DateTime holiday)
        {
            try
            {
                return _holidays.RemoveWhere(d => d.Date.Date.Equals(holiday.Date)) > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the provided DateTime is in our collection of holidays.
        /// </summary>
        /// <param name="dateTime">The date we would like to perform the check on.</param>
        /// <returns>True if it is a holiday.</returns>
        public static bool IsHoliday(DateTime dateTime)
        {
            return _holidays.Any(d => d.Date.Date == dateTime.Date);
        }

        /// <summary>
        /// Checks if the provided DateTime is on a weekend.
        /// </summary>
        /// <param name="dateTime">The date we would like to perform the check on.</param>
        /// <returns>True if the date is on a weekend.</returns>
        public static bool IsWeekend(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Checks if the provided DateTime falls within our specified business hours.
        /// </summary>
        /// <param name="dateTime">The date we would like to perform the check on.</param>
        /// <returns>True if the time is within business hours.</returns>
        public static bool IsWithinBusinessHours(DateTime dateTime)
        {
            return (dateTime.Hour > BusinessHoursStart.Hour && 
               dateTime.Minute > BusinessHoursStart.Minute &&
               dateTime.Second > BusinessHoursStart.Second) &&
               (dateTime.Hour < BusinessHoursEnd.Hour && 
               dateTime.Minute < BusinessHoursEnd.Minute && 
               dateTime.Second < BusinessHoursEnd.Second);
        }

        /// <summary>
        /// Checks if we are open on the provided DateTime, checking if we are open on holidays, weekends, or open 24hrs.
        /// </summary>
        /// <param name="dateTime">The date we would like to perform the check on.</param>
        /// <returns>True if the business is open.</returns>
        public static bool IsOpen(DateTime dateTime)
        {
            return IsAlwaysOpen || 
                ((IsOpenHolidays || !IsHoliday(dateTime)) && 
                (IsOpenWeekends || !IsWeekend(dateTime)) && 
                (IsOpen24Hours || IsWithinBusinessHours(dateTime)));
        }
    }
}
