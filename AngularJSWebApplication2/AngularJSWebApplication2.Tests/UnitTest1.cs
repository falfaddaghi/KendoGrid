using System;
using AngularJSWebApplication2.ServiceModel.Helpers;
using ServiceStack.Text;
using Xunit;
using System.Web;
namespace AngularJSWebApplication2.Tests
{    
    public class GroupParserTests
    {
        private string queryString =
            "take=40&skip=0&page=1&pageSize=40&group%5B0%5D%5Bfield%5D=SalesOrderID&group%5B0%5D%5Bdir%5D=asc&group%5B0%5D%5Baggregates%5D%5B0%5D%5Bfield%5D=LineTotal&group%5B0%5D%5Baggregates%5D%5B0%5D%5Baggregate%5D=sum";
        [Fact]
        public void FactMethodName()
        {
            var nvc = HttpUtility.ParseQueryString(queryString);
            var result = GroupHelper.Parse(nvc);

            result.PrintDump();
        }
    }
}
