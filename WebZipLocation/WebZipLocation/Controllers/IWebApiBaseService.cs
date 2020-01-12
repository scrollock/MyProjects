using ModelZipLocation;

namespace WebZipLocation.Controllers
{
    public interface IWebApiBaseService
    {
        void FillInformation(Location location, RequestVerb verb);
    }
}
