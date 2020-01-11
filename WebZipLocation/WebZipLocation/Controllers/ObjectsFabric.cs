using System;

namespace WebZipLocation.Controllers
{
    public class ObjectsFabric
    {
        public static T CreateObject<T>() where T: class
        {
            if (typeof(T).IsInterface)
                return CreateObjectFromInterface<T>() as T;
            return Activator.CreateInstance<T>();
        }

        private static object CreateObjectFromInterface<T>() where T : class
        {
            var typeT = typeof(T);
            if (typeT == typeof(ILocationService))
                return CreateObject<LocationService>();
            if (typeT == typeof(IWeatherMapService))
                return CreateObject<WeatherMapService>();
            if (typeT == typeof(IGoogleTimeZoneService))
                return CreateObject<GoogleTimeZoneService>();
            return new object();
        }
    }
}