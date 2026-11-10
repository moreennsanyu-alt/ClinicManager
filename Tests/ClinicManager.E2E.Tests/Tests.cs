using System.Diagnostics;
using DevExpress.EasyTest.Framework;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

[assembly: Parallelization(
    Mode = ParallelMode.None)]

// Refer to the following article for more information: https://docs.devexpress.com/eXpressAppFramework/403852/

namespace ClinicManager.E2E.Tests;

public class MainDemoTests
{
    [Fact]
    public void TestMethod()
    {
        
    }  
}
