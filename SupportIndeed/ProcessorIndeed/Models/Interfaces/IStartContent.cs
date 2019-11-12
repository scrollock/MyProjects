using ProcessorIndeed.Models.SupportDivision;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IStartContent
    {
        NetAddress PipeAddress { get; set; }
        Division Supportivision { get; set; }
        int TicketProcessingPeriodSecondes { get; set; }
        int StartManagerOffsetMinutes { get; set; }
        int StartDirectorOffsetMinutes { get; set; }
    }
}