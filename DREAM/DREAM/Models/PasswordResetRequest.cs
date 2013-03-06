﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Mail;

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
        public DateTime Timestamp { get; set; }
        public bool Enabled { get; set; }
	
        public MembershipUser User
        {
            get
            {
                return Membership.GetUser(UserID);
            }
        }

        //Return the new ID generated
        //If the ID already exists, generate a new ID 
	    public static long GenerateNewID()
        {
		    byte[] buffer = Guid.NewGuid().ToByteArray();
		    long id = BitConverter.ToInt64(buffer, 0);
            using (DREAMContext db = new DREAMContext())
            {
                if (db.PasswordResetRequests.Where(p => p.ID == id).Count() != 0)
                {
                        id = GenerateNewID();
                }
            }
		return id;
        }

        //Return the passwordResetRequest object for the given user
        //Sends an email to the user with a link to the PasswordResetRequest's page
	    public static PasswordResetRequest GenerateFor(MembershipUser user){
		    PasswordResetRequest resetReq = null;
		    using(DREAMContext db = new DREAMContext()) {
			    resetReq = new PasswordResetRequest();
			    {
				    resetReq.ID = PasswordResetRequest.GenerateNewID();
				    resetReq.UserID = (Guid)user.ProviderUserKey;
			    }
			    db.SaveChanges();
			   SendEmail("","","","","","");
		    return resetReq;
            }
         }

        // Sends an email address with the following properties
        // "from": Sender address
        // "to": Recepient address
        // "bcc": Bcc recepient
        // "cc": Cc recepient
        // "subject": Subject of mail message
        // "body": Body of mail message
        public static void SendEmail(string from, string to, string bcc, string cc, string subject, string body)
        {
            MailMessage mMailMessage = new MailMessage();

            mMailMessage.From = new MailAddress(from);
            mMailMessage.To.Add(new MailAddress(to));

            if ((bcc != null) && (bcc != string.Empty))
            {
                mMailMessage.Bcc.Add(new MailAddress(bcc));
            }  
            if ((cc != null) && (cc != string.Empty))
            {
                mMailMessage.CC.Add(new MailAddress(cc));
            }  
            mMailMessage.Subject = subject;
            mMailMessage.Body = body;

            mMailMessage.IsBodyHtml = true;
            mMailMessage.Priority = MailPriority.Normal;

            SmtpClient mSmtpClient = new SmtpClient();

            mSmtpClient.Send(mMailMessage);
        }
    }
}