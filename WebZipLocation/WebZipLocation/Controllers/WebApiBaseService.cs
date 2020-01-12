using ModelZipLocation;

namespace WebZipLocation.Controllers
{
    public abstract class WebApiBaseService : IWebApiBaseService
    {
        public abstract void FillInformation(Location location, RequestVerb verb);
        
    }
}