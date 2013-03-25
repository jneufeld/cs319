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
                new RequesterType { ID = id++, FullName = "Unknown", Code = "OTHER", enabled = true },
                new RequesterType { ID = id++, FullName = "Administrator", Code = "ADMIN", enabled = true },
                new RequesterType { ID = id++, FullName = "Dietician", Code = "RDN", enabled = true },
                new RequesterType { ID = id++, FullName = "Drug Company", Code = "DRUG CO", enabled = true },
                new RequesterType { ID = id++, FullName = "Family Member", Code = "FAMILY", enabled = true },
                new RequesterType { ID = id++, FullName = "Family Physician", Code = "GP", enabled = true },
                new RequesterType { ID = id++, FullName = "General Public", Code = "PUB", enabled = true },
                new RequesterType { ID = id++, FullName = "Librarian", Code = "LIB", enabled = true },
                new RequesterType { ID = id++, FullName = "Naturopath", Code = "ND", enabled = true },
                new RequesterType { ID = id++, FullName = "News Media", Code = "MEDIA", enabled = true },
                new RequesterType { ID = id++, FullName = "Nurse", Code = "RN", enabled = true },
                new RequesterType { ID = id++, FullName = "Oncologist", Code = "ONC", enabled = true },
                new RequesterType { ID = id++, FullName = "Other Health Care Professional", Code = "HCP", enabled = true },
                new RequesterType { ID = id++, FullName = "Patient", Code = "PATIENT", enabled = true },
                new RequesterType { ID = id++, FullName = "Pharmacist", Code = "RX", enabled = true },
                new RequesterType { ID = id++, FullName = "Public Relations", Code = "PR", enabled = true },
                new RequesterType { ID = id++, FullName = "Radiation therapist", Code = "RT", enabled = true },
                new RequesterType { ID = id++, FullName = "Researcher", Code = "RESEARCH", enabled = true },
                new RequesterType { ID = id++, FullName = "Social worker", Code = "SW", enabled = true },
                new RequesterType { ID = id++, FullName = "Student", Code = "STUDENT", enabled = true },
            };
            requesterTypes.ForEach(r => context.RequesterTypes.AddOrUpdate(r));

            id = 1;
            List<Region> regions = new List<Region>
            {
                new Region { ID = id++, FullName = "Not classified or unknown", Code = "UNK", enabled = true },
                new Region { ID = id++, FullName = "Alberta", Code = "AB", enabled = true },
                new Region { ID = id++, FullName = "Abbotsford Cancer Centre", Code = "AC", enabled = true },
                new Region { ID = id++, FullName = "BC - Not Classified/Unknown", Code = "BCUNK", enabled = true },
                new Region { ID = id++, FullName = "Centre for the Southern Interior", Code = "CSI", enabled = true },
                new Region { ID = id++, FullName = "Fraser Health Region", Code = "FHR", enabled = true },
                new Region { ID = id++, FullName = "Fraser Valley Region", Code = "FVC", enabled = true },
                new Region { ID = id++, FullName = "Interior Health Region", Code = "IHR", enabled = true },
                new Region { ID = id++, FullName = "Manitoba", Code = "MB", enabled = true },
                new Region { ID = id++, FullName = "New Brunswick", Code = "NB", enabled = true },
                new Region { ID = id++, FullName = "Newfoundland", Code = "NF", enabled = true },
                new Region { ID = id++, FullName = "Northern Health Region", Code = "NOR", enabled = true },
                new Region { ID = id++, FullName = "Nova Scotia", Code = "NS", enabled = true },
                new Region { ID = id++, FullName = "Northwest Territories", Code = "NT", enabled = true },
                new Region { ID = id++, FullName = "Nunavut", Code = "NU", enabled = true },
                new Region { ID = id++, FullName = "Ontario", Code = "ON", enabled = true },
                new Region { ID = id++, FullName = "Africa, Asia, Europe, USA", Code = "OTHER", enabled = true },
                new Region { ID = id++, FullName = "Prince Edward Island", Code = "PE", enabled = true },
                new Region { ID = id++, FullName = "Quebec", Code = "QC", enabled = true },
                new Region { ID = id++, FullName = "Saskatchewan", Code = "SK", enabled = true },
                new Region { ID = id++, FullName = "Vancouver Cancer Centre", Code = "VCC", enabled = true },
                new Region { ID = id++, FullName = "Vancouver Coastal Region", Code = "VCR", enabled = true },
                new Region { ID = id++, FullName = "Vancouver Island Centre", Code = "VIC", enabled = true },
                new Region { ID = id++, FullName = "Vancouver Island Region", Code = "VIHA", enabled = true },
                new Region { ID = id++, FullName = "Yukon", Code = "YK", enabled = true },
            };
            regions.ForEach(r => context.Regions.AddOrUpdate(r));

            id = 1;
            List<QuestionType> questionTypes = new List<QuestionType>
            {
                new QuestionType { ID = id++, FullName = "Other", Code = "OTHER", enabled = true },
                new QuestionType { ID = id++, FullName = "Adverse Effects", Code = "A/E", enabled = true },
                new QuestionType { ID = id++, FullName = "CAM", Code = "CAM", enabled = true },
                new QuestionType { ID = id++, FullName = "Coverage/Funding/Reimbursement", Code = "COST", enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Administration", Code = "ADMIN", enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Availability", Code = "AVAIL", enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Dosing", Code = "DOSE", enabled = true },
                new QuestionType { ID = id++, FullName = "Drug Interactions", Code = "DI", enabled = true },
                new QuestionType { ID = id++, FullName = "Therapy Selection", Code = "THER", enabled = true },
            };
            questionTypes.ForEach(q => context.QuestionTypes.AddOrUpdate(q));

            id = 1;
            List<TumourGroup> tumourGroups = new List<TumourGroup>
            {
                new TumourGroup { ID = id++, FullName = "General", Code = "GEN", enabled = true },
                new TumourGroup { ID = id++, FullName = "Breast", Code = "BR", enabled = true },
                new TumourGroup { ID = id++, FullName = "Central Nervous System", Code = "CNS", enabled = true },
                new TumourGroup { ID = id++, FullName = "Endocrine", Code = "ENDO", enabled = true },
                new TumourGroup { ID = id++, FullName = "Gastrointestinal", Code = "GI", enabled = true },
                new TumourGroup { ID = id++, FullName = "Genitourinary", Code = "GU", enabled = true },
                new TumourGroup { ID = id++, FullName = "Gynaecology", Code = "GO", enabled = true },
                new TumourGroup { ID = id++, FullName = "Head and Neck", Code = "H&N", enabled = true },
                new TumourGroup { ID = id++, FullName = "Leukemia", Code = "LEU", enabled = true },
                new TumourGroup { ID = id++, FullName = "Lung", Code = "LU", enabled = true },
                new TumourGroup { ID = id++, FullName = "Lymphoma", Code = "LY", enabled = true },
                new TumourGroup { ID = id++, FullName = "Melanoma", Code = "ME", enabled = true },
                new TumourGroup { ID = id++, FullName = "Multiple Myeloma", Code = "MM", enabled = true },
                new TumourGroup { ID = id++, FullName = "Ocular", Code = "OC", enabled = true },
                new TumourGroup { ID = id++, FullName = "Pediatrics", Code = "PED", enabled = true },
                new TumourGroup { ID = id++, FullName = "Primary Unknown", Code = "PU", enabled = true },
                new TumourGroup { ID = id++, FullName = "Sarcoma", Code = "SA", enabled = true },
                new TumourGroup { ID = id++, FullName = "Supportive Care", Code = "SUPP", enabled = true },
            };
            tumourGroups.ForEach(t => context.TumourGroups.AddOrUpdate(t));

            id = 1;
            List<Group> groups = new List<Group>
            {
                new Group { ID = id++, Code = "VC",  Name = "VANCOUVER CANCER CENTRE"}, 
                new Group { ID = id++, Code = "VIC", Name = "VANCOUVER ISLAND CENTRE"},
                new Group { ID = id++, Code = "AC", Name = "ABBOTSFORD CANCER CENTRE"},
                new Group { ID = id++, Code = "FVC", Name = "FRASER VALLEY CENTRE"},
                new Group { ID = id++, Code = "CSI", Name = "CENTRE FOR THE SOUTHERN INTERIOR"},
                new Group { ID = id++, Code = "CN", Name = "DON'T KNOW"},
                new Group { ID = id++, Code = "Provincial", Name = "PROVINCIAL"}
            };
            groups.ForEach(g => context.Groups.AddOrUpdate(g));

            foreach (MembershipUser user in Membership.GetAllUsers())
            {
                if (context.UserProfiles.Find((Guid)user.ProviderUserKey) == null)
                {
                    UserProfile up = new UserProfile
                    {
                        UserId = (Guid)user.ProviderUserKey,
                        FirstName = "First",
                        LastName = "Last",
                    };
                }
            }
        }
    }
}