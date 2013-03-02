using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Models
{
    public enum Gender
    {
        UNKNOWN,
        MALE,
        FEMALE,
    }

    public class Patient
    {
        int AgencyID;
        string FirstName;
        string LastName;
        Gender Gender;
        int Age;
    }
}