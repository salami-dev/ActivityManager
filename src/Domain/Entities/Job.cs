using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Entities;

public class Job : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Content { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public required DateTimeOffset EndDate { get; set; }

    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
}
