using System;
using Boilerplate.Common.Types.Interfaces;

namespace Boilerplate.Common.Types.Abstracts
{
    public abstract class SystemEntity : ISystemEntity
    {
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByUsername { get; set; }
        public string ModifiedById { get; set; }
        public string ModifiedByUsername { get; set; }
    }
}
