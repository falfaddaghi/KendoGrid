using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJSWebApplication1.ServiceInterface;
using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.Testing;
using ServiceStack.Text;

namespace AngularJSWebApplication2.AcceptanceTests.Infrastructure
{
    public class TestBase : AppHostHttpListenerBase
    {
        public string ListingOn { get; set; }        
        public JsonServiceClient Client { get; set; }
        public TestBase() : base("test",typeof(OrderService).Assembly)
        {
            ListingOn = "http://localhost:2203/";            
            base.Init();           
            base.Start(ListingOn);
            Client = new JsonServiceClient(ListingOn);
        }


        public override void Configure(Container container)
        {
            var conString = ConfigurationManager.ConnectionStrings["Portal"].ConnectionString;
            var conFactory = new OrmLiteConnectionFactory(conString, SqlServerDialect.Provider, true);
            container.Register<IDbConnectionFactory>(c => conFactory);
            this.Plugins.Add(new AutoQueryFeature());
            this.Plugins.Add(new RazorFormat());

        }
    }
}
