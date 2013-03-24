using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public enum LogAction
    {
        CREATE = 0,
        EDIT = 1,
        CLOSE = 2,
        VIEW = 3,
    }

    public class Log
    {
        public int ID { get; set; }
        public int Action { get; set; }
        public Guid UserID { get; set; }
        public int RequestID { get; set; }
        public DateTime TimePerformed { get; set; }

        public virtual Request Request { get; set; }

        public MembershipUser User
        {
            get
            {
                return Membership.GetUser(UserID);
            }
        }

        public static Log Create(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = (int)LogAction.CREATE,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
                TimePerformed = DateTime.UtcNow,
            };
            return log;
        }

        public static Log Edit(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = (int)LogAction.EDIT,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
                TimePerformed = DateTime.UtcNow,
            };
            return log;
        }

        public static Log Close(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = (int)LogAction.CLOSE,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
                TimePerformed = DateTime.UtcNow,
            };
            return log;
        }

        public static Log View(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = (int)LogAction.VIEW,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
                TimePerformed = DateTime.UtcNow,
            };
            return log;
        }
    }
}