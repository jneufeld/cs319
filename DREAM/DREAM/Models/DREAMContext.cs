using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public class DREAMContext : DbContext
    {
        public DREAMContext() : base("DefaultConnection") { }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Caller> Callers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<TumourGroup> TumourGroups { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<PreviousPassword> PreviousPasswords { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    }
}