using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace AngularJSWebApplication2.Tests.Infrastructure
{
    public class DomainCustomizationsAttribute : AutoDataAttribute
    {
        public DomainCustomizationsAttribute() : base(new Fixture().Customize(new DomainCustomizations()))
        {
            
        }
    }

    public class DomainCustomizations : CompositeCustomization
    {
        public DomainCustomizations() : base()
        {            
        }
    }
}
