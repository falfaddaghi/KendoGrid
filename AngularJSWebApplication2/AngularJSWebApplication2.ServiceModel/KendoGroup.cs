using System;
using System.Collections.Generic;

namespace AngularJSWebApplication2.ServiceModel
{
    public class KendoGroup
    {
        public string Field { get; set; }
        public object Aggregates { get; set; }
        public object Items { get; set; }
        public bool HasSubgroups { get; set; }
        public object Value { get; set; }
    }

}