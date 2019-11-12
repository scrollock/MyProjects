namespace ProcessorIndeed.Models.Interfaces
{
    public interface IContact : IObjectBase
    {
        string Email { get; set; }
        string Phone { get; set; }
        string About { get; set; }
    }
}
