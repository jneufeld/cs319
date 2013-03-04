using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class DREAMContext: DbContext
    {
        public DbSet<PreviousPassword> PreviousPasswords { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Caller> Callers { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DREAMContext() : base("DefaultConnection") {}
    }
}