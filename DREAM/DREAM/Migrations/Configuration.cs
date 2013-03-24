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
                new RequesterType { ID = id++, FullName = "Unknown", Code = "OTHER" },
                new RequesterType { ID = id++, FullName = "Administrator", Code = "ADMIN" },
                new RequesterType { ID = id++, FullName = "Dietician", Code = "RDN" },
                new RequesterType { ID = id++, FullName = "Drug Company", Code = "DRUG CO" },
                new RequesterType { ID = id++, FullName = "Family Member", Code = "FAMILY" },
                new RequesterType { ID = id++, FullName = "Family Physician", Code = "GP" },
                new RequesterType { ID = id++, FullName = "General Public", Code = "PUB" },
                new RequesterType { ID = id++, FullName = "Librarian", Code = "LIB" },
                new RequesterType { ID = id++, FullName = "Naturopath", Code = "ND" },
                new RequesterType { ID = id++, FullName = "News Media", Code = "MEDIA" },
                new RequesterType { ID = id++, FullName = "Nurse", Code = "RN" },
                new RequesterType { ID = id++, FullName = "Oncologist", Code = "ONC" },
                new RequesterType { ID = id++, FullName = "Other Health Care Professional", Code = "HCP" },
                new RequesterType { ID = id++, FullName = "Patient", Code = "PATIENT" },
                new RequesterType { ID = id++, FullName = "Pharmacist", Code = "RX" },
                new RequesterType { ID = id++, FullName = "Public Relations", Code = "PR" },
                new RequesterType { ID = id++, FullName = "Radiation therapist", Code = "RT" },
                new RequesterType { ID = id++, FullName = "Researcher", Code = "RESEARCH" },
                new RequesterType { ID = id++, FullName = "Social worker", Code = "SW" },
                new RequesterType { ID = id++, FullName = "Student", Code = "STUDENT" },
            };
            requesterTypes.ForEach(r => context.RequesterTypes.AddOrUpdate(r));

            id = 1;
            List<Region> regions = new List<Region>
            {
                new Region { ID = id++, FullName = "Not classified or unknown", Code = "UNK" },
                new Region { ID = id++, FullName = "Alberta", Code = "AB" },
                new Region { ID = id++, FullName = "Abbotsford Cancer Centre", Code = "AC" },
                new Region { ID = id++, FullName = "BC - Not Classified/Unknown", Code = "BCUNK" },
                new Region { ID = id++, FullName = "Centre for the Southern Interior", Code = "CSI" },
                new Region { ID = id++, FullName = "Fraser Health Region", Code = "FHR" },
                new Region { ID = id++, FullName = "Fraser Valley Region", Code = "FVC" },
                new Region { ID = id++, FullName = "Interior Health Region", Code = "IHR" },
                new Region { ID = id++, FullName = "Manitoba", Code = "MB" },
                new Region { ID = id++, FullName = "New Brunswick", Code = "NB" },
                new Region { ID = id++, FullName = "Newfoundland", Code = "NF" },
                new Region { ID = id++, FullName = "Northern Health Region", Code = "NOR" },
                new Region { ID = id++, FullName = "Nova Scotia", Code = "NS" },
                new Region { ID = id++, FullName = "Northwest Territories", Code = "NT" },
                new Region { ID = id++, FullName = "Nunavut", Code = "NU" },
                new Region { ID = id++, FullName = "Ontario", Code = "ON" },
                new Region { ID = id++, FullName = "Africa, Asia, Europe, USA", Code = "OTHER" },
                new Region { ID = id++, FullName = "Prince Edward Island", Code = "PE" },
                new Region { ID = id++, FullName = "Quebec", Code = "QC" },
                new Region { ID = id++, FullName = "Saskatchewan", Code = "SK" },
                new Region { ID = id++, FullName = "Vancouver Cancer Centre", Code = "VCC" },
                new Region { ID = id++, FullName = "Vancouver Coastal Region", Code = "VCR" },
                new Region { ID = id++, FullName = "Vancouver Island Centre", Code = "VIC" },
                new Region { ID = id++, FullName = "Vancouver Island Region", Code = "VIHA" },
                new Region { ID = id++, FullName = "Yukon", Code = "YK" },
            };
            regions.ForEach(r => context.Regions.AddOrUpdate(r));

            id = 1;
            List<QuestionType> questionTypes = new List<QuestionType>
            {
                new QuestionType { ID = id++, FullName = "Other", Code = "OTHER" },
                new QuestionType { ID = id++, FullName = "Adverse Effects", Code = "A/E" },
                new QuestionType { ID = id++, FullName = "CAM", Code = "CAM" },
                new QuestionType { ID = id++, FullName = "Coverage/Funding/Reimbursement", Code = "COST" },
                new QuestionType { ID = id++, FullName = "Drug Administration", Code = "ADMIN" },
                new QuestionType { ID = id++, FullName = "Drug Availability", Code = "AVAIL" },
                new QuestionType { ID = id++, FullName = "Drug Dosing", Code = "DOSE" },
                new QuestionType { ID = id++, FullName = "Drug Interactions", Code = "DI" },
                new QuestionType { ID = id++, FullName = "Therapy Selection", Code = "THER" },
            };
            questionTypes.ForEach(q => context.QuestionTypes.AddOrUpdate(q));

            id = 1;
            List<TumourGroup> tumourGroups = new List<TumourGroup>
            {
                new TumourGroup { ID = id++, FullName = "General", Code = "GEN" },
                new TumourGroup { ID = id++, FullName = "Breast", Code = "BR" },
                new TumourGroup { ID = id++, FullName = "Central Nervous System", Code = "CNS" },
                new TumourGroup { ID = id++, FullName = "Endocrine", Code = "ENDO" },
                new TumourGroup { ID = id++, FullName = "Gastrointestinal", Code = "GI" },
                new TumourGroup { ID = id++, FullName = "Genitourinary", Code = "GU" },
                new TumourGroup { ID = id++, FullName = "Gynaecology", Code = "GO" },
                new TumourGroup { ID = id++, FullName = "Head and Neck", Code = "H&N" },
                new TumourGroup { ID = id++, FullName = "Leukemia", Code = "LEU" },
                new TumourGroup { ID = id++, FullName = "Lung", Code = "LU" },
                new TumourGroup { ID = id++, FullName = "Lymphoma", Code = "LY" },
                new TumourGroup { ID = id++, FullName = "Melanoma", Code = "ME" },
                new TumourGroup { ID = id++, FullName = "Multiple Myeloma", Code = "MM" },
                new TumourGroup { ID = id++, FullName = "Ocular", Code = "OC" },
                new TumourGroup { ID = id++, FullName = "Pediatrics", Code = "PED" },
                new TumourGroup { ID = id++, FullName = "Primary Unknown", Code = "PU" },
                new TumourGroup { ID = id++, FullName = "Sarcoma", Code = "SA" },
                new TumourGroup { ID = id++, FullName = "Supportive Care", Code = "SUPP" },
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