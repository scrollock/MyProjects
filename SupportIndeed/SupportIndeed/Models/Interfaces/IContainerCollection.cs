using ProcessorIndeed.Models.Documents;
using System.Collections.Generic;

namespace SupportIndeed.Models.Interfaces
{
    interface IContainerCollection
    {
        ICollection<Ticket> Collection { get; set; }
    }
}
