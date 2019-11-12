using ProcessorIndeed.Models.Interfaces;
using ProcessorIndeed.Models.SupportDivision;
using System.Drawing;

namespace ProcessorIndeed.Models.Emploees
{
    public class UnitEmploee : ObjectBase<IUnitEmploee>, IUnitEmploee
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public Division Division{get; set;}
        public Contact Contact { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Image UnitImage { get; set; }

    }
}