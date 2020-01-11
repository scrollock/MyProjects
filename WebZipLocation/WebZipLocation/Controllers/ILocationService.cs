using ModelZipLocation;

namespace WebZipLocation.Controllers
{
    public interface ILocationService
    {
        void FillInformation(Location location, RequestVerb verb);
    }
}