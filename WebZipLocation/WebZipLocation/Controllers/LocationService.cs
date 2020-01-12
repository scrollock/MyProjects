using ModelZipLocation;
using System;
using System.Threading.Tasks;
using WebZipLocation.Models;

namespace WebZipLocation.Controllers
{
    public class LocationService : WebApiBaseService, ILocationService
    {
        public override void FillInformation(Location location, RequestVerb verb)
        {
            try
            {
                var weatherMap = StartProcessing(ProcessingStartWeatherMap, location);
                weatherMap.Wait();
                var googleTimeZone = StartProcessing(ProcessingGoogleTimeZone, location);
                googleTimeZone.Wait();
                location.FrandlyMessage = $@"At the location {location.CityName}, the temperature is {location.CurrentTemperature}, and the timezone is {location.TimeZone}";
                //Task.WaitAll(weatherMap, googleTimeZone);
            }
            catch (AggregateException ae)
            {
                var resultError = ErrorHelper.ProcessingErrors(ae, location);
                if (!string.IsNullOrEmpty(resultError))
                {
                    location.ErrorMessage += resultError + StaticConstants.ColoneSpace;
                }
            }
        }
        private Task StartProcessing(Action<Location> callbackDelegate, Location location)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    callbackDelegate(location);
                }
                catch (Exception ex)
                {
                    location.Exceptions.Add(ex);
                    location.ErrorMessage += ex.Message + StaticConstants.ColoneSpace;
                }
            });
        }
        
        private void ProcessingStartWeatherMap(Location location)
        {
            var service = ObjectsFabric.CreateObject<IWeatherMapService>();
            service.FillInformation(location, RequestVerb.GET);
        }
        private void ProcessingGoogleTimeZone(Location location)
        {
            var service = ObjectsFabric.CreateObject<IGoogleTimeZoneService>();
            service.FillInformation(location, RequestVerb.GET);
        }
    }
}