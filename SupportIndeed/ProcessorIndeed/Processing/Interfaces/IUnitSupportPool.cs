using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.Interfaces;
using System.Collections.Generic;

namespace ProcessorIndeed.Processing.Interfaces
{
    public interface IUnitSupportPool
    {
        IEnumerable<IPosition> Pool{ get; set; }
        IPosition GetIdleOperator();
        IPosition GetIdleOperatorOrManager();
        IPosition GetDirector();
    }
}
