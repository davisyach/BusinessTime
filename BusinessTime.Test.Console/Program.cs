using BusinessTime.Calculator;
using System;
using System.Threading.Tasks;

namespace BusinessTime.Test.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeZone = Task.Run(() => TimeZoneConverter.GetTimeZoneInfo(null, null, "80202")).Result;

            Console.WriteLine(timeZone.DisplayName);
            Console.ReadKey();
        }
    }
}
