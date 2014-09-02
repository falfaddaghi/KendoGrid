using System.Collections.Generic;
using ServiceStack;

namespace AngularJSWebApplication2.ServiceModel
{
    public class KendoGridBaseRequest<T> : QueryBase<T> 
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string Logic { get; set; }

        //public FilterObjectWrapper FilterObjectWrapper { get; set; }
        //public IEnumerable<SortObject> SortObjects { get; set; }
        public IEnumerable<Group> GroupObjects { get; set; } 
    }    
}