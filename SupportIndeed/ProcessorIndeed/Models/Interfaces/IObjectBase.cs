using System;

namespace ProcessorIndeed.Models.Interfaces
{
    public interface IObjectBase
    {
        Guid id { get; set; }
        Type TypeObject { get; }
        bool Deleted { get; set; }
        Guid? PermissionId { get; set; }
    }
}
