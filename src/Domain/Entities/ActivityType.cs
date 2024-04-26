using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Entities;

public class ActivityType : BaseAuditableEntity
{
    public required string Name { get; set; }
}
