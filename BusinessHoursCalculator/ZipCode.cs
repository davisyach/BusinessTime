using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessTime
{
    public class ZipCode
    {
        public string Zip;
        public string City;
        public string State;
        public double Latitude;
        public double Longitude;
        public int TimeZoneOffset;
        public bool ObservesDST;

        public static ZipCode Current;


        Google.Maps.TimeZone.TimeZoneRequest request = new Google.Maps.TimeZone.TimeZoneRequest();

        public void temp()
        {
            
        }
        
    }
}
