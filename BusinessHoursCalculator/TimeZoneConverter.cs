using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Maps;
using Google.Maps.Geocoding;
using Google.Maps.TimeZone;

namespace BusinessTime.Calculator
{
    public class TimeZoneConverter
    {
        private const string APIKEY = "AIzaSyDUBnZe8_h7BRrXM1x5fx3hsO0CnYisXig";

        public static async Task<TimeZoneInfo> GetTimeZoneInfoAsync(string city = null, string state = null, string zipCode = null)
        {
            GoogleSigned.AssignAllServices(new GoogleSigned(APIKEY));

            string address = "";

            if(zipCode != null)
            {
                address = zipCode;
            }
            else
            {
                address = $"{city}, {state} {zipCode}";
            }

            var geoRequest = new GeocodingRequest();
            geoRequest.Address = address;

            var geoResponse = await new GeocodingService().GetResponseAsync(geoRequest);

            if(geoResponse.Status == ServiceResponseStatus.Ok && geoResponse.Results.Count() > 0)
            {
                var result = geoResponse.Results.First();

                var tzRequest = new TimeZoneRequest();
                tzRequest.Timestamp = DateTime.Now;
                tzRequest.Location = result.Geometry.Location;

                var tzResponse = await new TimeZoneService().GetResponseAsync(tzRequest);

                if(tzResponse.Status == ServiceResponseStatus.Ok)
                {
                    return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(t => new string[]{ t.Id, t.StandardName, t.DaylightName}.Contains(tzResponse.TimeZoneName));
                }
            }

            return null;
        }

        public static TimeZoneInfo GetTimeZoneInfo(string city = null, string state = null, string zipCode = null)
        {
            GoogleSigned.AssignAllServices(new GoogleSigned(APIKEY));

            string address = "";

            if (zipCode != null)
            {
                address = zipCode;
            }
            else
            {
                address = $"{city}, {state} {zipCode}";
            }

            var geoRequest = new GeocodingRequest();
            geoRequest.Address = address;

            var geoResponse = new GeocodingService().GetResponse(geoRequest);

            if (geoResponse.Status == ServiceResponseStatus.Ok && geoResponse.Results.Count() > 0)
            {
                var result = geoResponse.Results.First();

                var tzRequest = new TimeZoneRequest();
                tzRequest.Timestamp = DateTime.Now;
                tzRequest.Location = result.Geometry.Location;

                var tzResponse = new TimeZoneService().GetResponse(tzRequest);

                if (tzResponse.Status == ServiceResponseStatus.Ok)
                {
                    return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(t => new string[] { t.Id, t.StandardName, t.DaylightName }.Contains(tzResponse.TimeZoneName));
                }
            }

            return null;
        }
    }
}
