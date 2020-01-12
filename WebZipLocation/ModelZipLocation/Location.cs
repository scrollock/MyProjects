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
        public Coord Coord { get; set; }
        public string Key { get; set; }
        public string FrandlyMessage { get; set; }
        
        [NonSerialized]
        public ICollection<Exception> Exceptions = new List<Exception>();
    }
}
