using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace DREAM.Models
{
    public class PreviousPassword
    {
        public int ID { get; set; }
	    [Required]
	    public MembershipPasswordFormat PasswordFormat { get; set; }
	    [Required]
	    public string PasswordSalt { get; set; }
	    [Required]
	    public string Password { get; set; }
	    [Required]
	    public Guid UserID { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime Timestamp { get; set; }

        public MembershipUser User
        {
            get
            {
                return Membership.GetUser(UserID);
            }
        }

        //Returns true if the password can be used by the user,
        //false if the password has been recently used by the user.
	    public static bool CheckPassword(MembershipUser user, string password) 
        {
		    string hashAlgorithm = Membership.HashAlgorithmType;
		    DateTime checkPasswordsAfter = DateTime.Now - new TimeSpan(253, 0, 0, 0);
            using(DREAMContext db = new DREAMContext())
            {
			    foreach(PreviousPassword prevPwd in db.PreviousPasswords.Where(p => p.UserID.Equals((Guid)user.ProviderUserKey) && p.Timestamp > checkPasswordsAfter))
                {
                    string passwordHash = prevPwd.EncodePassword(password, prevPwd.PasswordSalt);
				    if(passwordHash == prevPwd.Password)
					    return false;
                }
            }
		    return true;
        }
        //Returns a hash of the given password and password salt. 
        public string EncodePassword(string pass, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            string hashAlgorithm = Membership.HashAlgorithmType;
            HashAlgorithm algorithm = HashAlgorithm.Create(hashAlgorithm);
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }
    }
}