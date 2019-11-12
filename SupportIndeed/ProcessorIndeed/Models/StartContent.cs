using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;

namespace ProcessorIndeed.Models
{
    public class StartContent : IStartContent
    {
        public Division Supportivision { get; set;}
        public NetAddress PipeAddress { get; set; }
        public int TicketProcessingPeriodSecondes { get; set; }
        public int StartManagerOffsetMinutes { get; set; }
        public int StartDirectorOffsetMinutes { get; set; }
    }
}
