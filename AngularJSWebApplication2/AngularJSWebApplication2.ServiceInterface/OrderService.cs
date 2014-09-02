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


namespace AngularJSWebApplication1.ServiceInterface
{
    public class OrderService : Service
    {
        public object Any(SalesOrderDetail req)
        {
            return new SalesOrderDetailResponse { OrderDetails = Db.Select<SalesOrderDetail>().ToList() };
        }

    }


    public class SalesOrderService : Service
    {
        public IAutoQuery AutoQuery { get; set; }
        public object Any(FindOrders req)
        {
            //what to group by
           var grouping = GroupHelper.Parse(Request.QueryString.ToNameValueCollection());

            var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
            //q.GroupBy(x=>x.SalesOrderID).
            var result = AutoQuery.Execute(req, q);
            var groupBy = result.Results.GroupBy(x => x.SalesOrderID).ToList();
            var grouped = groupBy.Skip(req.Skip.GetValueOrDefault(0)).Take(req.Take.GetValueOrDefault(10));
            result.Total = groupBy.Count();

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

            var response = result.ConvertTo<KendoGridResponse<SalesOrderDetail>>();            
                       
            response.Groups = groupResponse;                        
            return response;
        }

        public object Any(FindGroupHeader req)
        {
            var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
            q.Select(x=>x.SalesOrderID).GroupBy(x => x.SalesOrderID).OrderBy(x => x.SalesOrderID).Skip(req.Skip.GetValueOrDefault(0)).Take(req.Take.GetValueOrDefault(10));
            var result = AutoQuery.Execute(req, q);

            return result;
        } 
        public object Any(FindGroupDetails req)
        {
            var q = AutoQuery.CreateQuery(req, Request.GetRequestParams());
            var result = AutoQuery.Execute(req, q);
            var sum = result.Results.Aggregate((0.0), (x, y) => x += y.LineTotal).ToString();
            result.Results = result.Results.Skip(req.Skip.GetValueOrDefault(0)).Take(req.Take.GetValueOrDefault(3)).ToList();            
            result.Meta = result.Meta?? new Dictionary<string, string>();
            result.Meta.Add("Sum",sum);
            return result;
        }
    }
}