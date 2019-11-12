using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.SupportDivision;
using System;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface ITicket : IObjectBase
    {
        string Title { get; set; }
        string Body { get; set; }
        Position OwnerPosition { get; set; }
        LevelPositionEnum CurrentLewelOwner { get; set; }
        bool IsCanceled { get; set; }
        bool IsCompleted { get; set; }
        DateTime StartProcessing { get; set; }
        DateTime EndProcessing { get; set; }
        TimeSpan Period { get; set; }
        object Lock { get; }
    }
}
