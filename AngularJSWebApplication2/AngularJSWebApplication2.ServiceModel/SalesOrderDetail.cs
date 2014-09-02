using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace AngularJSWebApplication2.ServiceModel
{
    [Route("/Orders")]
    [Schema("Sales")]
    public class SalesOrderDetail : IReturn<SalesOrderDetailResponse>
    {
        public string SalesOrderID { get; set; }
        public string SalesOrderDetailID { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public string OrderQty { get; set; }
        public string ProductID { get; set; }
        public string SpecialOfferID { get; set; }
        public string UnitPrice { get; set; }
        public string UnitPriceDiscount { get; set; }
        public double LineTotal { get; set; }
        public string rowguid { get; set; }
        public string ModifiedDate { get; set; }
        [Ignore]
        public double Sum { get; set; }
    }
   
    public class SalesOrderDetailResponse
    {
       public List<SalesOrderDetail> OrderDetails { get; set; } 
    }

    [Route("/SalesOrders")]
    public class FindOrders : KendoGridBaseRequest<SalesOrderDetail> { }
    [Route("/SalesOrders/Grouped")]
    public class FindGroupHeader : KendoGridBaseRequest<SalesOrderDetail> { }
    [Route("/SalesOrders/Details")]
    public class FindGroupDetails : KendoGridBaseRequest<SalesOrderDetail> { }


    }