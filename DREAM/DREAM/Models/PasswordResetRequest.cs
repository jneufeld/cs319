using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using System.Security;
using System.Security.Cryptography;

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

        // adapted from http://stackoverflow.com/questions/3132878/asp-net-membership-provider-reset-password-features-email-confirmation-and-p
        public static String CreateResetPasswordHash(int lengthOfHash)
        {
            var random = new Random();
            var chars = new char[lengthOfHash];
            const string AllowableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/^()";
            
            var charsToUse = string.Format("{0}{1}", AllowableCharacters, "");
            var allowableLength = charsToUse.Length;
            
            for (var i = 0; i < lengthOfHash; i++)
            {
                chars[i] = charsToUse[random.Next(allowableLength)];
            }
     
            return new String(chars);
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

                String passwordLinkHashValue = CreateResetPasswordHash(24);

			   SendEmail("souffle.dream@gmail.com",user.Email,"","","DREAM Password Reset",passwordLinkHashValue);
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
            mMailMessage.To.Add(to);

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

            //mMailMessage.IsBodyHtml = true;
            //mMailMessage.Priority = MailPriority.Normal;

            SmtpClient mSmtpClient = new SmtpClient("smtp.gmail.com", 587);
            mSmtpClient.EnableSsl = true;
            mSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mSmtpClient.UseDefaultCredentials = true;

            // need to take out
            SecureString emailPassword = new SecureString();
            
            foreach(char c in "319_TeamOne".ToCharArray())
            {
                emailPassword.AppendChar(c);
            }

            mSmtpClient.Credentials = new System.Net.NetworkCredential(from, emailPassword);

            mSmtpClient.Send(mMailMessage);


            // can add attachment to email through a similar line (taken from http://stackoverflow.com/questions/12786154/send-email-through-gmail-smtp)
            // mMailMessage.Attachments.Add(new System.Net.Mail.Attachment(path)); 

            //mSmtpClient.Host = "Need to determine a smtpServer";
            //String SmtpUserName = "Need to determine this";
            //String SmtpPassword = "Need to determine this";
            //SecureString SmtpPassword = new SecureString("Need to determine this");

            //if (SmtpUserName != null && SmtpPassword != null)
            //{
            //    mSmtpClient.UseDefaultCredentials = false;

            //    mSmtpClient.Credentials = new NetworkCredential(SmtpUserName, SmtpPassword);
            //}
            //else
            //{
            //    mSmtpClient.UseDefaultCredentials = true;
                //mSmtpClient.Credentials.GetCredential(mSmtpClient.Host, 25, "NTMP");
                //mSmtpClient.Credentials = (System.Net.ICredentialsByHost)System.Net.CredentialCache.DefaultNetworkCredentials;
                //mSmtpClient.Credentials = (System.Net.ICredentialsByHost)System.Net.CredentialCache.DefaultCredentials;
            //}

            //mSmtpClient.Send(mMailMessage);
        }
    }
}