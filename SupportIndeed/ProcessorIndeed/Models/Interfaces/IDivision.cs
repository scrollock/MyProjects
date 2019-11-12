using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.SupportDivision;
using System.Collections.Generic;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IDivision : IObjectBase
    {
        string DivisionName { get; set; }
        Division Parent { get; set; }
        IEnumerable<Position> Positions { get; set; }
        UnitEmploee Lider { get; set; }
    }
}
