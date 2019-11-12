using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.Interfaces;
using System.Collections.Generic;

namespace ProcessorIndeed.Models.SupportDivision
{
    public class Division : ObjectBase<IDivision>, IDivision
    {
        public string DivisionName { get; set; }
        public Division Parent { get; set; }
        public IEnumerable<Position> Positions {get; set;}
        public UnitEmploee Lider { get; set; } 
    }
}