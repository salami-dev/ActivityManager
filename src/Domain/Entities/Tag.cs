using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Entities;

public class Tag : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Theme { get; set; } = String.Empty;

    public ICollection<Activity> Activities { get; } = [];
    public ICollection<Job> Jobs { get; } = [];
}
