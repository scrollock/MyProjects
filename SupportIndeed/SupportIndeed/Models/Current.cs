﻿using ProcessorIndeed.Models.Documents;
using SupportIndeed.Models.Interfaces;
using System.Collections.Generic;

namespace SupportIndeed.Models
{
    public class Current : IContainerCollection
    {
        public ICollection<Ticket> Collection { get; set; }
    }
}