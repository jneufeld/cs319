using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class Group
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }

        /*
        public virtual List<MembershipUser> Users
        {
            get
            {
                return new List<MembershipUser>();
            }
        }
        */

        public static void CreateDefaultGroups()
        {
            Dictionary<string, string> groupData = new Dictionary<string, string> {
                {"VC", "VANCOUVER CANCER CENTRE"}, 
                {"VIC", "VANCOUVER ISLAND CENTRE"},
                {"AC", "ABBOTSFORD CANCER CENTRE"},
                {"FVC", "FRASER VALLEY CENTRE"},
                {"CSI", "CENTRE FOR THE SOUTHERN INTERIOR"},
                {"CN", "DON'T KNOW"},
                {"Provincial", "PROVINCIAL"}
            };
            foreach (KeyValuePair<string, string> groupPair in groupData)
            {
                using (DREAMContext db = new DREAMContext())
                {
                    if (db.Groups.Where(g => g.Name == groupPair.Value && g.Code == groupPair.Key).Count() == 0)
                    {
                        Group group = new Group
                        {
                            Code = groupPair.Key,
                            Name = groupPair.Value,
                        };
                        db.Groups.Add(group);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}