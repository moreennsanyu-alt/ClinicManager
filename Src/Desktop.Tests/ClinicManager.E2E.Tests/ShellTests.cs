using NUnit.Framework;
using ClinicManager.E2E.Tests.Core;

namespace ClinicManager.E2E.Tests
{
    public class ShellTests : UITestBase
    {
        [Test]
        public void shell_window_is_sizable()
        {
            var window = Application.GetMainWindow(Automation);
            //var calc = (OperatingSystem.IsWindows10() || OperatingSystem.IsWindows11()) ? (ICalculator)new Win10Calc(window) : new LegacyCalc(window);
        }

    }
}
