using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace DREAM.Models
{
    public class DREAMContextInitializer : DropCreateDatabaseIfModelChanges<DREAMContext>
    {
        protected override void Seed(DREAMContext context)
        {
        }
    }
}