using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DREAM.Models;

namespace DREAM.Controllers
{
    public class AutocompleteController : Controller
    {
        private DREAMContext db = new DREAMContext();

        //
        // POST: /Autocomplete/Keyword/

        [HttpPost]
        public JsonResult Keyword(string prefix)
        {
            IEnumerable<string> response = db.Keywords
                .Where(k => k.KeywordText.StartsWith(prefix))
                .Select(k => k.KeywordText);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private class PatientCompletionModel
        {
            public string id { get { return AgencyID; } }
            public string label
            {
                get { return FirstName + " " + LastName + " - " + AgencyID; }
            }
            public string value { get { return AgencyID; } }

            public string AgencyID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Gender { get; set; }
            public int Age { get; set; }
        }

        [HttpPost]
        public JsonResult Patient(string agencyId)
        {
            IEnumerable<Patient> results = db.Patients
                .Where(p => p.AgencyID.StartsWith(agencyId));
            IEnumerable<PatientCompletionModel> response = results.ToList()
                .Select(p => new PatientCompletionModel
                {
                    AgencyID = p.AgencyID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = ((Gender)p.Gender).ToString(),
                    Age = p.Age
                });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
