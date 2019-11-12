using ProcessorIndeed.Models.Interfaces;

namespace ProcessorIndeed.Models.Emploees
{
    public class Contact : ObjectBase<IContact>, IContact
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string About { get; set; }
    }
}