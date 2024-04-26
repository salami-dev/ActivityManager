using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Entities;

public class Activity : BaseAuditableEntity
{
    public Activity()
    {
        Tags = new HashSet<Tag>();
    }

    public required string Name { get; set; }
    public required string Description { get; set; } = string.Empty;
    public string? Url { get; set; }
    public DateTimeOffset? StartDate { get; set; } = null;
    public DateTimeOffset? EndDate { get; set; } = null;

    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;

    public int ActivityTypeId { get; set; }
    public ActivityType ActivityType { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; }
}
