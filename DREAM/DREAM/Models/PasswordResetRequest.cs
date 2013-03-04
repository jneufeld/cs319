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
        public long ID;
	    [Required]
	    public MembershipUser User;
	    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime timestamp;
	    public bool Enabled = true;
	
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
				    resetReq.User = user;
			    }
			    db.SaveChanges();
			   // send email to user with a link to the PasswordResetRequest’s page;
		    return resetReq;
            }
         }
    }
}