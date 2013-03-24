using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class HomeViewModel
    {
        public IList<RequestViewModel> OpenRequests { get; set; }
        public IList<RequestViewModel> RecentRequests { get; set; }
    }
}