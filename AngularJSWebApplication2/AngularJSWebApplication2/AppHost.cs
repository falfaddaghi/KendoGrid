using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Funq;
using AngularJSWebApplication2.ServiceInterface;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack;
using ServiceStack.Text;

namespace AngularJSWebApplication2  
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("AngularJSWebApplication1", typeof(MyServices).Assembly)
        {
        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
            JsConfig.EmitCamelCaseNames = true;
            SetConfig(new HostConfig
            {
                AppendUtf8CharsetOnContentTypes = new HashSet<string> { MimeTypes.Html },
                AllowJsonpRequests = true,
                AllowFileExtensions = { "json", "cshtml" } 
            }
          );
            var conString = ConfigurationManager.ConnectionStrings["Portal"].ConnectionString;
            var conFactory = new OrmLiteConnectionFactory(conString, SqlServerDialect.Provider, true);
            container.Register<IDbConnectionFactory>(c => conFactory);
            this.Plugins.Add(new AutoQueryFeature());
            this.Plugins.Add(new RazorFormat());
        }
    }
}