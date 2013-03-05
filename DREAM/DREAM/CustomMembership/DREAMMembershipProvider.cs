using DREAM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace DREAM.CustomMembership
{
    public class DREAMMembershipProvider : SqlMembershipProvider
    {
        public static Tuple<MembershipPasswordFormat, string, string> ExtractPasswordData(MembershipUser user)
        {
            MembershipPasswordFormat passwordFormat;
            string passwordSalt;
            string password;

            ConnectionStringSettings connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"];
            using(SqlConnection conn = new SqlConnection(connectionString.ConnectionString))
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT PasswordFormat, PasswordSalt, Password FROM aspnet_Membership WHERE UserId=@UserId";
                    cmd.Parameters.AddWithValue("@UserId", user.ProviderUserKey);
                    conn.Open();
                    using(SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if(rdr != null && rdr.Read())
                        {
                            passwordFormat = (MembershipPasswordFormat) rdr.GetInt32(0);
                            passwordSalt = rdr.GetString(1);
                            password = rdr.GetString(2);
                        }
                        else
                        {
                            throw new Exception("Error extracting current password data from the database.");
                        }
                    }
                }
            }

            return new Tuple<MembershipPasswordFormat, string, string>(passwordFormat, passwordSalt, password);
        }

        // creates and save a PreviousPassword object for the given user's password data
        private void recordCurrentPassword(MembershipUser user)
        {
            Tuple<MembershipPasswordFormat, string, string> currentPasswordData = ExtractPasswordData(user);
            using(DREAMContext db = new DREAMContext())
            {
                PreviousPassword prevPwd = new PreviousPassword
                {
                    UserID = (Guid)user.ProviderUserKey,
                    PasswordFormat = currentPasswordData.Item1,
                    PasswordSalt = currentPasswordData.Item2,
                    Password = currentPasswordData.Item3
                };
                db.PreviousPasswords.Add(prevPwd);
                db.SaveChanges();
            }
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipUser user = base.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);

            if (user!=null)
            {
                recordCurrentPassword(user);
            }

            return user;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            MembershipUser user = Membership.GetUser(username);

            if(!PreviousPassword.CheckPassword(user, newPassword))
            {
                throw new PreviouslyUsedPasswordException();
            }

            bool success = base.ChangePassword(username, oldPassword, newPassword);

            if(success)
            {
                recordCurrentPassword(user);
            }

            return success;
        }
    }
}