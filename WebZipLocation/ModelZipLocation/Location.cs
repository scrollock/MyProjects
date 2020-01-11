using System;
using System.Collections.Generic;
using System.ComponentModel;
using WebZipLocation.Models;

namespace ModelZipLocation
{
    public class Location
    {
        public string ZipCode { get; set; }
        public string CityName { get; set; }
        public string CurrentTemperature { get; set; }
        public string TimeZone { get; set; }
        public string ErrorMessage = string.Empty;
        public string FrandlyMessage
        {
            get => $@"At the location {CityName}, the temperature is {CurrentTemperature}, and the timezone is {ZipCode}";
        }
        [NonSerialized]
        public ICollection<Exception> Exceptions = new List<Exception>();
    }
}
