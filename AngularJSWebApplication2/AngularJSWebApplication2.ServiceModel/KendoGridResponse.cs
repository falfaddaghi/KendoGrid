using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace AngularJSWebApplication2.ServiceModel
{
  public class KendoGridResponse<T> : QueryResponse<T>
  {
      public IEnumerable<KendoGroup> Groups { get; set; } 
      //later to add filter and sort
    }
}
