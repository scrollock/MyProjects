using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.SupportDivision;
using System;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IPosition : IObjectBase
    {
        string PositionName { get; set; }
        Guid? SupportDivisionId { get; set; }
        UnitEmploee Lider { get; set; }
        UnitEmploee Unit { get; set; }
        LevelPositionEnum Level { get; set; }
        bool IsWorkBusy { get; set; }
        DateTime? StartIdle { get; set; }
        object Lock { get; }
    }
}
