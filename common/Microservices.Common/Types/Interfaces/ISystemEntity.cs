using System;

namespace Microservices.Common.Types.Interfaces
{
    public interface ISystemEntity
    {
        string CreatedById { get; set; }
        string CreatedByUsername { get; set; }
        DateTime CreationDate { get; set; }
        string ModifiedById { get; set; }
        string ModifiedByUsername { get; set; }
        DateTime? ModifiedDate { get; set; }
    }
}
