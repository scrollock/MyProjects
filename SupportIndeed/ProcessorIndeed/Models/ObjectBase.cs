using ProcessorIndeed.Models.Interfaces;
using System;

namespace ProcessorIndeed.Models
{
    public abstract class ObjectBase<T> : IObjectBase where T : class
    {
        public Guid id { get; set; }
        public Type TypeObject => typeof(T);
        public bool Deleted { get; set; }
        public Guid? PermissionId { get; set; }
    }
}