using ModelZipLocation;
using System.Configuration;


namespace WebZipLocation.Controllers
{
    public class GoogleTimeZoneService : LocationService, IGoogleTimeZoneService
    {
        public override void FillInformation(Location location, RequestVerb verb)
        {
            var lingual = ConfigurationManager.AppSettings["Lingual"];
            var weatherMapUrl = ConfigurationManager.AppSettings["WeatherMapUrl"];
        }
    }
}