using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DREAM.Models;
using System.Web.Security;

namespace DREAM.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DREAMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DREAMContext context)
        {
            //  This method will be called after migrating to the latest version.

            int id = 1;
            List<RequesterType> requesterTypes = new List<RequesterType>
            {
                new RequesterType { ID = id++, FullName = "Unknown", Code = "OTHER", Enabled = true },
                new RequesterType { ID = id++, FullName = "Administrator", Code = "ADMIN", Enabled = true },
                new RequesterType { ID = id++, FullName = "Dietician", Code = "RDN", Enabled = true },
                new RequesterType { ID = id++, FullName = "Drug Company", Code = "DRUG CO", Enabled = true },
                new RequesterType { ID = id++, FullName = "Family Member", Code = "FAMILY", Enabled = true },
                new RequesterType { ID = id++, FullName = "Family Physician", Code = "GP", Enabled = true },
                new RequesterType { ID = id++, FullName = "General Public", Code = "PUB", Enabled = true },
                new RequesterType { ID = id++, FullName = "Librarian", Code = "LIB", Enabled = true },
                new RequesterType { ID = id++, FullName = "Naturopath", Code = "ND", Enabled = true },
                new RequesterType { ID = id++, FullName = "News Media", Code = "MEDIA", Enabled = true },
                new RequesterType { ID = id++, FullName = "Nurse", Code = "RN", Enabled = true },
                new RequesterType { ID = id++, FullName = "Oncologist", Code = "ONC", Enabled = true },
                new RequesterType { ID = id++, FullName = "Other Health Care Professional", Code = "HCP", Enabled = true },
                new RequesterType { ID = id++, FullName = "Patient", Code = "PATIENT", Enabled = true },
                new RequesterType { ID = id++, FullName = "Pharmacist", Code = "RX", Enabled = true },
                new RequesterType { ID = id++, FullName = "Public Relations", Code = "PR", Enabled = true },
                new RequesterType { ID = id++, FullName = "Radiation therapist", Code = "RT", Enabled = true },
                new RequesterType { ID = id++, FullName = "Researcher", Code = "RESEARCH", Enabled = true },
                new RequesterType { ID = id++, FullName = "Social worker", Code = "SW", Enabled = true },
                new RequesterType { ID = id++, FullName = "Student", Code = "STUDENT", Enabled = true },
            };
            requesterTypes.ForEach(r => context.RequesterTypes.AddOrUpdate(r));

            id = 1;
            List<Region> regions = new List<Region>
            {
                new Region { ID = id++, FullName = "Not classified or unknown", Code = "UNK", Enabled = true },
                new Region { ID = id++, FullName = "Alberta", Code = "AB", Enabled = true },
                new Region { ID = id++, FullName = "Abbotsford Cancer Centre", Code = "AC", Enabled = true },
                new Region { ID = id++, FullName = "BC - Not Classified/Unknown", Code = "BCUNK", Enabled = true },
                new Region { ID = id++, FullName = "Centre for the Southern Interior", Code = "CSI", Enabled = true },
                new Region { ID = id++, FullName = "Fraser Health Region", Code = "FHR", Enabled = true },
                new Region { ID = id++, FullName = "Fraser Valley Region", Code = "FVC", Enabled = true },
                new Region { ID = id++, FullName = "Interior Health Region", Code = "IHR", Enabled = true },
                new Region { ID = id++, FullName = "Manitoba", Code = "MB", Enabled = true },
                new Region { ID = id++, FullName = "New Brunswick", Code = "NB", Enabled = true },
                new Region { ID = id++, FullName = "Newfoundland", Code = "NF", Enabled = true },
                new Region { ID = id++, FullName = "Northern Health Region", Code = "NOR", Enabled = true },
                new Region { ID = id++, FullName = "Nova Scotia", Code = "NS", Enabled = true },
                new Region { ID = id++, FullName = "Northwest Territories", Code = "NT", Enabled = true },
                new Region { ID = id++, FullName = "Nunavut", Code = "NU", Enabled = true },
                new Region { ID = id++, FullName = "Ontario", Code = "ON", Enabled = true },
                new Region { ID = id++, FullName = "Africa, Asia, Europe, USA", Code = "OTHER", Enabled = true },
                new Region { ID = id++, FullName = "Prince Edward Island", Code = "PE", Enabled = true },
                new Region { ID = id++, FullName = "Quebec", Code = "QC", Enabled = true },
                new Region { ID = id++, FullName = "Saskatchewan", Code = "SK", Enabled = true },
                new Region { ID = id++, FullName = "Vancouver Cancer Centre", Code = "VCC", Enabled = true },
                new Region { ID = id++, FullName = "Vancouver Coastal Region", Code = "VCR", Enabled = true },
                new Region { ID = id++, FullName = "Vancouver Island Centre", Code = "VIC", Enabled = true },
                new Region { ID = id++, FullName = "Vancouver Island Region", Code = "VIHA", Enabled = true },
                new Region { ID = id++, FullName = "Yukon", Code = "YK", Enabled = true },
            };
            regions.ForEach(r => context.Regions.AddOrUpdate(r));

            id = 1;
            List<QuestionType> questionTypes = new List<QuestionType>
            {
                new QuestionType { ID = id++, FullName = "Other", Code = "OTHER", Enabled = true },
                new QuestionType { ID = id++, FullName = "Adverse Effects", Code = "A/E", Enabled = true },
                new QuestionType { ID = id++, FullName = "CAM", Code = "CAM", Enabled = true },
                new QuestionType { ID = id++, FullName = "Coverage/Funding/Reimbursement", Code = "COST", Enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Administration", Code = "ADMIN", Enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Availability", Code = "AVAIL", Enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Dosing", Code = "DOSE", Enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Interactions", Code = "DI", Enabled = true },
                new QuestionType { ID = id++, FullName = "Therapy Selection", Code = "THER", Enabled = true },
            };
            questionTypes.ForEach(q => context.QuestionTypes.AddOrUpdate(q));

            id = 1;
            List<TumourGroup> tumourGroups = new List<TumourGroup>
            {
                new TumourGroup { ID = id++, FullName = "General", Code = "GEN", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Breast", Code = "BR", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Central Nervous System", Code = "CNS", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Endocrine", Code = "ENDO", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Gastrointestinal", Code = "GI", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Genitourinary", Code = "GU", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Gynaecology", Code = "GO", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Head and Neck", Code = "H&N", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Leukemia", Code = "LEU", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Lung", Code = "LU", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Lymphoma", Code = "LY", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Melanoma", Code = "ME", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Multiple Myeloma", Code = "MM", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Ocular", Code = "OC", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Pediatrics", Code = "PED", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Primary Unknown", Code = "PU", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Sarcoma", Code = "SA", Enabled = true },
                new TumourGroup { ID = id++, FullName = "Supportive Care", Code = "SUPP", Enabled = true },
            };
            tumourGroups.ForEach(t => context.TumourGroups.AddOrUpdate(t));

            id = 1;
            List<Group> groups = new List<Group>
            {
                new Group { ID = id++, Code = "AC", Name = "Abbottsford Centre"},
                new Group { ID = id++, Code = "CN", Name = "Centre for the North"},
                new Group { ID = id++, Code = "CSI", Name = "Centre for the Southern Interior"},
                new Group { ID = id++, Code = "FVC", Name = "Fraser Valley Centre"},
                new Group { ID = id++, Code = "VC",  Name = "Vancouver Centre"}, 
                new Group { ID = id++, Code = "VIC", Name = "Vancouver Island Centre"},
                new Group { ID = id++, Code = "Prov", Name = "Provincial"},
            };
            groups.ForEach(g => context.Groups.AddOrUpdate(g));

            // add a default admin user            
            if (Membership.GetUser("admin") == null)
            {
                Membership.CreateUser("admin", "Admin1!", "admin@example.com");
                Roles.AddUsersToRoles(new string[] {"admin"}, new string[] {Role.ADMIN, Role.DI_SPECIALIST, Role.REPORTER, Role.VIEWER});
            }
        }
    }
}