using ProcessorIndeed.Models.Emploees;
using ProcessorIndeed.Models.SupportDivision;
using System.Drawing;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IUnitEmploee : IObjectBase
    {
        string Name { get; set; }
        string SecondName { get; set; }
        Division Division { get; set; }
        Contact Contact { get; set; }
        string Login { get; set; }
        string PasswordHash { get; set; }
        Image UnitImage { get; set; }
    }
}
