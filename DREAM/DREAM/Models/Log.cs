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
	    public LogAction Action { get; set; }
	    public Guid UserID { get; set; }
	    public int RequestID { get; set; }

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
                Action = LogAction.CREATE,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
            };
            return log;
        }

        public static Log Edit(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = LogAction.EDIT,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
            };
            return log;
        }

        public static Log Close(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = LogAction.CLOSE,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
            };
            return log;
        }

        public static Log View(Request request, MembershipUser user)
        {
            Log log = new Log
            {
                Action = LogAction.VIEW,
                UserID = (Guid)user.ProviderUserKey,
                RequestID = request.ID,
            };
            return log;
        }
    }
}