using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;
using ProcessorIndeed.Processing.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProcessorIndeed.Processing
{
    public class UnitSupportPool : IUnitSupportPool
    {
        public void InitPool(IList<IPosition> pool)
        {
            Pool = pool;
        }
        public IEnumerable<IPosition> Pool { get; set; }

        public IPosition GetDirector()
        {
            if (Pool == null)
                return null;
            return Pool.FirstOrDefault(x=>x.Level == LevelPositionEnum.Director);
        }

        public IPosition GetIdleOperator()
        {
            return Pool.Where(x => x.Level == LevelPositionEnum.Operator && !x.IsWorkBusy).OrderBy(x=>x.StartIdle).FirstOrDefault();
        }

        public IPosition GetIdleOperatorOrManager()
        {
            return Pool.Where(x => (x.Level == LevelPositionEnum.Operator || x.Level == LevelPositionEnum.Manager) && !x.IsWorkBusy)
                .OrderBy(x => x.StartIdle).FirstOrDefault(); 
        }
    }
}
