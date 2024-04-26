using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Application.Tags.Commands;
public record NewOrExistingTag
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Theme { get; set; }
}
