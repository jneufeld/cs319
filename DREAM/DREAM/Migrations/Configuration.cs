using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using DREAM.Models;

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

            List<RequestType> requestTypes = new List<RequestType>
            {
                new RequestType { FullName = "Unknown", Code = "OTHER" },
                new RequestType { FullName = "Administrator", Code = "ADMIN" },
                new RequestType { FullName = "Dietician", Code = "RDN" },
                new RequestType { FullName = "Drug Company", Code = "DRUG CO" },
                new RequestType { FullName = "Family Member", Code = "FAMILY" },
                new RequestType { FullName = "Family Physician", Code = "GP" },
                new RequestType { FullName = "General Public", Code = "PUB" },
                new RequestType { FullName = "Librarian", Code = "LIB" },
                new RequestType { FullName = "Naturopath", Code = "ND" },
                new RequestType { FullName = "News Media", Code = "MEDIA" },
                new RequestType { FullName = "Nurse", Code = "RN" },
                new RequestType { FullName = "Oncologist", Code = "ONC" },
                new RequestType { FullName = "Other Health Care Professional", Code = "HCP" },
                new RequestType { FullName = "Patient", Code = "PATIENT" },
                new RequestType { FullName = "Pharmacist", Code = "RX" },
                new RequestType { FullName = "Public Relations", Code = "PR" },
                new RequestType { FullName = "Radiation therapist", Code = "RT" },
                new RequestType { FullName = "Researcher", Code = "RESEARCH" },
                new RequestType { FullName = "Social worker", Code = "SW" },
                new RequestType { FullName = "Student", Code = "STUDENT" },
            };
            requestTypes.ForEach(r => context.RequestTypes.AddOrUpdate(r));

            List<Region> regions = new List<Region>
            {
                new Region { FullName = "Not classified or unknown", Code = "UNK" },
                new Region { FullName = "Alberta", Code = "AB" },
                new Region { FullName = "Abbotsford Cancer Centre", Code = "AC" },
                new Region { FullName = "BC - Not Classified/Unknown", Code = "BCUNK" },
                new Region { FullName = "Centre for the Southern Interior", Code = "CSI" },
                new Region { FullName = "Fraser Health Region", Code = "FHR" },
                new Region { FullName = "Fraser Valley Region", Code = "FVC" },
                new Region { FullName = "Interior Health Region", Code = "IHR" },
                new Region { FullName = "Manitoba", Code = "MB" },
                new Region { FullName = "New Brunswick", Code = "NB" },
                new Region { FullName = "Newfoundland", Code = "NF" },
                new Region { FullName = "Northern Health Region", Code = "NOR" },
                new Region { FullName = "Nova Scotia", Code = "NS" },
                new Region { FullName = "Northwest Territories", Code = "NT" },
                new Region { FullName = "Nunavut", Code = "NU" },
                new Region { FullName = "Ontario", Code = "ON" },
                new Region { FullName = "Africa, Asia, Europe, USA", Code = "OTHER" },
                new Region { FullName = "Prince Edward Island", Code = "PE" },
                new Region { FullName = "Quebec", Code = "QC" },
                new Region { FullName = "Saskatchewan", Code = "SK" },
                new Region { FullName = "Vancouver Cancer Centre", Code = "VCC" },
                new Region { FullName = "Vancouver Coastal Region", Code = "VCR" },
                new Region { FullName = "Vancouver Island Centre", Code = "VIC" },
                new Region { FullName = "Vancouver Island Region", Code = "VIHA" },
                new Region { FullName = "Yukon", Code = "YK" },
            };
            regions.ForEach(r => context.Regions.AddOrUpdate(r));

            List<QuestionType> questionTypes = new List<QuestionType>
            {
                new QuestionType { FullName = "Other", Code = "OTHER" },
                new QuestionType { FullName = "Adverse Effects", Code = "A/E" },
                new QuestionType { FullName = "CAM", Code = "CAM" },
                new QuestionType { FullName = "Coverage/Funding/Reimbursement", Code = "COST" },
                new QuestionType { FullName = "Drug Administration", Code = "ADMIN" },
                new QuestionType { FullName = "Drug Availability", Code = "AVAIL" },
                new QuestionType { FullName = "Drug Dosing", Code = "DOSE" },
                new QuestionType { FullName = "Drug Interactions", Code = "DI" },
                new QuestionType { FullName = "Therapy Selection", Code = "THER" },
            };
            questionTypes.ForEach(q => context.QuestionTypes.AddOrUpdate(q));

            List<TumourGroup> tumourGroups = new List<TumourGroup>
            {
                new TumourGroup { FullName = "General", Code = "GEN" },
                new TumourGroup { FullName = "Breast", Code = "BR" },
                new TumourGroup { FullName = "Central Nervous System", Code = "CNS" },
                new TumourGroup { FullName = "Endocrine", Code = "ENDO" },
                new TumourGroup { FullName = "Gastrointestinal", Code = "GI" },
                new TumourGroup { FullName = "Genitourinary", Code = "GU" },
                new TumourGroup { FullName = "Gynaecology", Code = "GO" },
                new TumourGroup { FullName = "Head and Neck", Code = "H&N" },
                new TumourGroup { FullName = "Leukemia", Code = "LEU" },
                new TumourGroup { FullName = "Lung", Code = "LU" },
                new TumourGroup { FullName = "Lymphoma", Code = "LY" },
                new TumourGroup { FullName = "Melanoma", Code = "ME" },
                new TumourGroup { FullName = "Multiple Myeloma", Code = "MM" },
                new TumourGroup { FullName = "Ocular", Code = "OC" },
                new TumourGroup { FullName = "Pediatrics", Code = "PED" },
                new TumourGroup { FullName = "Primary Unknown", Code = "PU" },
                new TumourGroup { FullName = "Sarcoma", Code = "SA" },
                new TumourGroup { FullName = "Supportive Care", Code = "SUPP" },
            };
            tumourGroups.ForEach(t => context.TumourGroups.AddOrUpdate(t));
        }
    }
}
