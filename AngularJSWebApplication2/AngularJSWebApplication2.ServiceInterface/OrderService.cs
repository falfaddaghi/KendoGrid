using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Amib.Threading;
using AngularJSWebApplication2.ServiceModel;
using AngularJSWebApplication2.ServiceModel.Helpers;
using ServiceStack;
using ServiceStack.OrmLite;
using ServiceStack.Web;
using System.Linq.Dynamic;

namespace AngularJSWebApplication1.ServiceInterface
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
            //StatisticManager.SaveUserAgent(userAgent);
        }

        public override void Execute(IRequest req, IResponse res, object requestDto)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderService : Service
    {
        public interface IHasRequestFilter
        {
            void RequestFilter(IHttpRequest req, IHttpResponse res, object requestDto);
            int Priority { get; } // <0 Run before global filters. >=0 Run after
            IHasRequestFilter Copy(); // Optimization: fast creation of new instances
        }

        public interface IHasResponseFilter
        {
            void ResponseFilter(IHttpRequest req, IHttpResponse res, object responseDto);
            int Priority { get; } // <0 Run before global filters. >=0 Run after
            IHasResponseFilter Copy(); // Optimization: fast creation of new instances
        }

        

        public class SalesOrderService : Service
        {



            public IAutoQuery AutoQuery { get; set; }

            public object Any(FindOrders req)
            {
               



                var grouping = GroupHelper.Parse(Request.QueryString.ToNameValueCollection());
                var sorting = SortHelper.Parse(Request.QueryString.ToNameValueCollection());
                var result = Request.GetRequestParams();
                if (sorting != null)
                {
                    


                    var first = sorting.First();
                    if (first.Logic == "and")
                    {
                        foreach (var aa in first.Filters)
                        {
                            if (aa != null && aa.Any())
                            {
                                String sql = "";
                                var logic = aa.First();
                                // in case field has space
                                logic.Field = logic.Field.Replace(" ", "");
                                if (logic.Operator == "eq")
                                    result.Add(logic.Field, logic.Value);
                                else if (logic.Operator == "neq")
                                {
                                    typeof (KendoGridBaseRequest<SalesOrderDetail>)
                                        .GetProperty(logic.Field + "Temp")
                                        .AddAttributes(new QueryFieldAttribute { Template = "{Field} != {Value}", Field = logic.Field });
                                   
                                    typeof (KendoGridBaseRequest<SalesOrderDetail>)
                                        .GetProperty(logic.Field + "Temp").SetValue(req, logic.Value);
                                }
                                else if (logic.Operator == "startswith")
                                    result.Add(logic.Field + "StartsWith", logic.Value);
                                else if (logic.Operator == "contains")
                                    result.Add(logic.Field + "Contains", logic.Value);
                                else if (logic.Operator == "doesnotcontain")
                                {
                                    typeof(KendoGridBaseRequest<SalesOrderDetail>)
                                        .GetProperty(logic.Field + "Temp")
                                        .AddAttributes(new QueryFieldAttribute { Template = "UPPER({Field}) LIKE %UPPER({Value})%", Field = logic.Field });

                                    typeof(KendoGridBaseRequest<SalesOrderDetail>)
                                        .GetProperty(logic.Field + "Temp").SetValue(req, logic.Value);
                                }
                                else if (logic.Operator == "endswith")
                                    result.Add(logic.Field + "EndsWith", logic.Value);
                                
                            }
                        }
                    }
                    else if (first.Logic == "or")
                    {

                    }
                    
                }
                req.AllPrameterDictionary = result;


                
            

                // return sorting;
                //var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
                var q = AutoQuery.CreateQuery(req, req.AllPrameterDictionary);
                
                //q.GroupBy(x=>x.SalesOrderID).
                var resultA = AutoQuery.Execute(req, q);
                
                var groupBy = resultA.Results.GroupBy(x => x.SalesOrderID).ToList();
                var grouped = groupBy.Skip(req.Skip.GetValueOrDefault(0)).Take(req.Take.GetValueOrDefault(10));
                resultA.Total = groupBy.Count();

                //project into a kendoGroup and calculate the Aggregate  
                var groupResponse = new List<KendoGroup>();
                foreach (var group in grouped)
                {
                    var sum = @group.Sum(salesOrderDetail => Convert.ToDouble(salesOrderDetail.LineTotal));
                    foreach (var salesOrderDetail in group)
                    {
                        salesOrderDetail.Sum = sum;
                    }
                    var aggDic = new Dictionary<string, string>();
                    aggDic.Add("sum", sum.ToString());
                    var kendoGroup = new KendoGroup
                    {
                        Field = "salesOrderId",
                        HasSubgroups = false,
                        Items = group,
                        Aggregates = aggDic,
                        Value = group.Key
                    };
                    groupResponse.Add(kendoGroup);
                }
                /* Here is the Sorting happening */
              //  KendoGridResponse<SalesOrderDetail> response = null;
                string data = Request.QueryString.Get("sort[0][field]");
                List<SalesOrderDetail> SalesList = null;
                if (data != null)
                {
                    
                    string direction = Request.QueryString.Get("sort[0][dir]");
                    if (direction != null)
                    {
                       
                        if (direction == "desc")
                        {
                            SalesList = resultA.ConvertTo<KendoGridResponse<SalesOrderDetail>>().Results.OrderBy(data).ToList();
                            SalesList.Reverse();
                        }
                        else
                            SalesList = resultA.ConvertTo<KendoGridResponse<SalesOrderDetail>>().Results.OrderBy(data).ToList();
                    }

                }

              
                    var response = resultA.ConvertTo<KendoGridResponse<SalesOrderDetail>>();
                    response.Results = SalesList ?? response.Results;

                response.Groups = groupResponse;
                return response;
            }

            public object Any(FindGroupHeader req)
            {
                var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
                q.Select(x => x.SalesOrderID)
                    .GroupBy(x => x.SalesOrderID)
                    .OrderBy(x => x.SalesOrderID)
                    .Skip(req.Skip.GetValueOrDefault(0))
                    .Take(req.Take.GetValueOrDefault(10));
                var result = AutoQuery.Execute(req, q);

                return result;
            }

            public object Any(FindGroupDetails req)
            {
                var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
                var result = AutoQuery.Execute(req, q);
                var sum = result.Results.Aggregate((0.0), (x, y) => x += y.LineTotal).ToString();
                result.Results =
                    result.Results.Skip(req.Skip.GetValueOrDefault(0)).Take(req.Take.GetValueOrDefault(3)).ToList();
                result.Meta = result.Meta ?? new Dictionary<string, string>();
                result.Meta.Add("Sum", sum);
                return result;
            }
        }
    }

}