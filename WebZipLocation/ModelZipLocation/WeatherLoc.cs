using System.Collections.Generic;

namespace ModelZipLocation
{
    public class WeatherLoc
    {
        public Coord coord { get; set; }
        public List<WeatherPoint> weather { get; set; }
        public string Base {get;set;}
        public WeatherMain main { get; set; }
        public long visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public long dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
        public string message { get; set; }
    }
}
