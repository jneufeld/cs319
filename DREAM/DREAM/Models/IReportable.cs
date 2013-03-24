using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public interface IReportable
    {
        DateTime CreationTime { get; }
        DateTime? CompletionTime { get; }
    }
}