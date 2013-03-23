using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DREAM.Attributes
{
    public class StratifiableAttribute : Attribute
    {
        public string Name = null;

        public StratifiableAttribute() { }

        public StratifiableAttribute(string name)
        {
            Name = name;
        }
    }
}