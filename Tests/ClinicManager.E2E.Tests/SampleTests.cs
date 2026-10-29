using System.Diagnostics;
using DevExpress.EasyTest.Framework;
using Xunit;
using Xunit.v3;
using Xunit.Sdk;

[assembly: Parallelization(Mode = ParallelMode.None)]

// Refer to the following article for more information: https://docs.devexpress.com/eXpressAppFramework/403852/

namespace ClinicManager.E2E.Tests;
public class ClinicManagerTests : IDisposable {
  
    const string WinAppName = "ClinicManagerWin";
    EasyTestFixtureContext FixtureContext { get; } = new EasyTestFixtureContext();

    public ClinicManagerTests() {
        FixtureContext.RegisterApplications(
            new WinApplicationOptions(WinAppName, @$"{Environment.CurrentDirectory}\..\..\..\..\MainDemo.Win\bin\EasyTest\MainDemo.Win.exe"));
       // FixtureContext.RegisterDatabases(new DatabaseOptions(MainDemoDBName, "MainDemo.NET.XPO_v26.1", server: "(localdb)\\mssqllocaldb"));
    }
    public void Dispose() {
        FixtureContext.CloseRunningApplications();
        DropDB();
      //FixtureContext.DropDB(MainDemoDBName);
        // Temporary fix for VS2022 17.14 because it doesn't execute Application.ApplicationExited event after process.CloseMainWindow()
        // and so we cannot stop the MiddleTier server after the test. The problem is reproduced only with xUnit.v3 and Microsoft.Testing.Platform.
        Process existProc = Process.GetProcessesByName("ClinicMgrSvr").FirstOrDefault();
        if(existProc != null) {
            existProc.Kill();
            existProc.WaitForExit();
        }
    }
    public void DropDB()
    {
      
    }
  
    [Fact]
    public void CreateNewRole() {
        DropDB();
        var appContext = FixtureContext.CreateApplicationContext(WinAppName);
        appContext.RunApplication();
        appContext.GetForm().FillForm(("User Name", "Sam"));
        appContext.GetAction("Log In").Execute();
        appContext.Navigate("Roles");

        appContext.GetAction("New").Execute();
        appContext.GetAction("Save").Execute();

        Assert.Equal("\"Name\" must not be empty.", appContext.GetValidation().GetValidationMessages().First());

        if(appContext.IsWin()) {
            appContext.GetAction("Close").Execute();
        }
        appContext.GetForm().FillForm(("Name", "TechWriter"));
        appContext.GetAction("Save and Close").Execute();

        Assert.True(appContext.GetGrid().RowExists(("Name", "TechWriter")));
    }

 

}
