using NUnit.Framework;
using ClinicManager.E2E.Tests.Core;

namespace ClinicManager.E2E.Tests
{
    // [TestFixture] is optional in NUnit 3+ unless using generic or parameterized tests
    public class WindowTests : UITestBase
    {
        
        [Test]
        public void title_should_contain_company_name()
        {
           var window = Application.GetMainWindow(Automation);
          
        }

    }
}

