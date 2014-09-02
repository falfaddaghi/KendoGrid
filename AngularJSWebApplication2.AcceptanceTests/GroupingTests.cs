using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJSWebApplication2.AcceptanceTests.Infrastructure;
using AngularJSWebApplication2.ServiceModel;
using ServiceStack;
using Xunit;

namespace AngularJSWebApplication2.AcceptanceTests
{
   public class GroupingTests : TestBase
    {
       private string queryString =
           "take=40&skip=0&page=1&pageSize=40&group%5B0%5D%5Bfield%5D=SalesOrderID&group%5B0%5D%5Bdir%5D=asc&group%5B0%5D%5Baggregates%5D%5B0%5D%5Bfield%5D=LineTotal&group%5B0%5D%5Baggregates%5D%5B0%5D%5Baggregate%5D=sum";
       

       [Fact]
       public void GroupingWorks()
       {
           var url = new FindOrders().ToPostUrl() + "?" + queryString;
           Client.Get(url);
       }
    }
}
