using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;
using System;

namespace ProcessorIndeed.Models.Emploees
{
    public class Position : ObjectBase<IPosition>, IPosition
    {
        public string PositionName { get; set; }
        public Guid? SupportDivisionId { get; set; }
        public UnitEmploee Lider { get; set; }
        public UnitEmploee Unit { get; set; }
        public LevelPositionEnum Level {get; set;}
        public bool IsWorkBusy { get; set; }
        public DateTime? StartIdle { get; set; }
        object IPosition.Lock => new object();
    }
}