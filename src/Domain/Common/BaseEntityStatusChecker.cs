using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManager.Domain.Common;
public class BaseEntityStatusChecker
{
    public int? Id { get; set; }

    public bool IsNew()
    {
        if (Id == null || !Id.HasValue || Id == 0) return true;

        return false;
    }

    public virtual BaseEntity GetNewModel()
    {
        throw new NotImplementedException();

    }
}
