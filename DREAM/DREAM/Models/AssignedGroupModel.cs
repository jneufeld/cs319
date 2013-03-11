using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class AssignedGroupModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public bool Assigned { get; set; }
    }
}