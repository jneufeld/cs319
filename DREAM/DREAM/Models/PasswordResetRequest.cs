using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DREAM.Models
{
    public class PasswordResetRequest
    {
        public PasswordResetRequest()
        {
            this.Enabled = true;
        }

        public long ID { get; set; }
	    [Required]
	    public Guid UserID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime timestamp { get; set; }
        public bool Enabled { get; set; }
	
        public MembershipUser User
        {
            get
            {
                return Membership.GetUser(UserID);
            }
        }

	    public static long GenerateNewID()
        {
		    byte[] buffer = Guid.NewGuid().ToByteArray();
		    long id = BitConverter.ToInt64(buffer, 0);
		   // if(another PasswordResetRequests have the same id)
			 //   id = GenerateNewID();
		return id;
        }

	    public static PasswordResetRequest GenerateFor(MembershipUser user){
		    PasswordResetRequest resetReq = null;
		    using(DREAMContext db = new DREAMContext()) {
			    resetReq = new PasswordResetRequest();
			    {
				    resetReq.ID = PasswordResetRequest.GenerateNewID();
				    resetReq.UserID = (Guid)user.ProviderUserKey;
			    }
			    db.SaveChanges();
			   // send email to user with a link to the PasswordResetRequest’s page;
		    return resetReq;
            }
         }
    }
}