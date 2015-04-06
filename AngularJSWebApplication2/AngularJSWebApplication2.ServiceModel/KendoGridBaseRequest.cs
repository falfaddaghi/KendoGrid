using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace AngularJSWebApplication2.ServiceModel
{
    public class KendoGridBaseRequest<T> : QueryBase<T> 
    {
        

        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Logic { get; set; }

        public Dictionary<string, string> AllPrameterDictionary { get; set; }

      /*  [QueryField(Template = "{Field} = 2",
            Field = "salesOrderID")]
*/
        public string salesOrderIDTemp { get; set; }
        public string salesOrderDetailIDTemp { get; set; }
        public string carrierTrackingNumberTemp { get; set; }
        public string orderQtyTemp { get; set; }
        public string productIDTemp { get; set; }
        public string specialOfferIDTemp { get; set; }
        public string unitPriceTemp { get; set; }
     //   public string unitPriceDiscount { get; set; }
     //   public double lineTotal { get; set; }
     //   public string rowguid { get; set; }
     //   public string modifiedDate { get; set; }


     /*   [QueryField(Template = "{Field} = {Value}",
                Field = "SalesOrderId")]
        public string SalesOrderId { get; set; }
        [QueryField(Template = "{Field} != {Value}",
                Field = "SalesOrderId")]
        public string SalesOrderIdNotEqual { get; set; }
         [QueryField(Template = "{Field} LIKE {Value}",
                Field = "FirstName", ValueFormat = "{0}%")]
        public string StartsWith { get; set; }
        //public FilterObjectWrapper FilterObjectWrapper { get; set; }
        public IEnumerable<SortObject> SortObjects { get; set; }
        public IEnumerable<Group> GroupObjects { get; set; } */
    }    
}