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
    }
}