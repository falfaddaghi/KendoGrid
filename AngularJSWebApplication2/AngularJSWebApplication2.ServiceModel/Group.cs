using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using AngularJSWebApplication2.ServiceModel.Helpers;
using ServiceStack;
using ServiceStack.Web;

namespace AngularJSWebApplication2.ServiceModel
{
    public interface IHasRequestFilter
    {
        void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto);
        int Priority { get; }      // <0 Run before global filters. >=0 Run after
        IHasRequestFilter Copy();  // Optimization: fast creation of new instances
    }
    public interface IHasResponseFilter
    {
        void ResponseFilter(IHttpRequest req, IHttpResponse res, object responseDto);
        int Priority { get; }      // <0 Run before global filters. >=0 Run after
        IHasResponseFilter Copy();  // Optimization: fast creation of new instances
    }
    public class CustomRequestFilterAttribute : RequestFilterAttribute
    {
        public void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            //This code is executed before the service
            string userAgent = req.UserAgent;
            //req.FormData.Add("salesOrderDetailIDStartsWith", "m");
            req.SetItem("salesOrderDetailIDStartsWith", "m");
            
            //StatisticManager.SaveUserAgent(userAgent);
        }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            /*FindOrders finalOrderObject = (FindOrders)requestDto;
            var sorting = SortHelper.Parse(req.QueryString.ToNameValueCollection());
            if (sorting != null)
            {
                var result = req.GetRequestParams();
                

                var first = sorting.First();
                if (first.Logic == "and")
                {
                    foreach (var aa in first.Filters)
                    {
                        if (aa != null && aa.Any())
                        {
                            String sql = "";
                            var logic = aa.First();
                        if (logic.Operator == "eq")
                            result.Add(logic.Field + "StartsWith", logic.Value);
                       else if(logic.Operator == "neq")
                          result.Add(logic.Field + "StartsWith", logic.Value);
                       else if (logic.Operator == "startswith")
                            result.Add(logic.Field + "StartsWith", logic.Value);
                       else if(logic.Operator == "contains")
                            result.Add(logic.Field + "StartsWith", logic.Value);
                       else if (logic.Operator == "doesnotcontain")
                            result.Add(logic.Field + "StartsWith", logic.Value);
                       else if (logic.Operator == "endswith")
                            result.Add(logic.Field + "StartsWith", logic.Value);
                            //  if (logic.Operator == "eq")
                            //req.PopulateWith()
                            /* else if (logic.Operator == "neq")
                           req.SalesOrderIdNotEqual = logic.Value;
                       else if (logic.Operator == "startswith")
                           req.SalesOrderIdNotEqual = logic.Value;
                       else if (logic.Operator == "contains")
                           req.SalesOrderIdNotEqual = logic.Value;
                       else if (logic.Operator == "doesnotcontain")
                           req.SalesOrderIdNotEqual = logic.Value;
                       else if (logic.Operator == "endswith")
                           req.SalesOrderIdNotEqual = logic.Value;#1#
                        }
                    }
                }
                else if (first.Logic == "or")
                {

                }

            }
            

           
            finalOrderObject.AllPrameterDictionary = result;
            //  reqa.Skip = 500;

            //  req.RequestAttributes = new RequestAttributes();
            //res.Redirect(string.Format("{0}", req.AbsoluteUri));
            //throw new NotImplementedException();
        }
*/
            }
    }
    public class Group
    {
        public string Field { get; set; }

        public string Direction { get; set; }
        public IEnumerable<AggregateObject> Aggregates { get; set; }
    }
    public class AggregateObject
    {
        public string Field { get; set; }
        public string Aggregate { get; set; }

        /// <summary>
        /// Get the Linq Aggregate
        /// </summary>
        /// <param name="fieldConverter">Convert which can be used to convert ViewModel to Model if needed.</param>
        /// <returns>string</returns>
        public string GetLinqAggregate(Func<string, string> fieldConverter = null)
        {
            string convertedField = fieldConverter != null ? fieldConverter.Invoke(Field) : Field;
            switch (Aggregate)
            {
                case "count":
                    return string.Format("Count() as count__{0}", Field);

                case "sum":
                    return string.Format("Sum(TEntity__.{0}) as sum__{1}", convertedField, Field);

                case "max":
                    return string.Format("Max(TEntity__.{0}) as max__{1}", convertedField, Field);

                case "min":
                    return string.Format("Min(TEntity__.{0}) as min__{1}", convertedField, Field);

                case "average":
                    return string.Format("Average(TEntity__.{0}) as average__{1}", convertedField, Field);

                default:
                    return string.Empty;
            }
        }
    }
}