using FlaUI.Core;
using FlaUI.Core.Capturing;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using FlaUI.TestUtilities;
using System;
using System.Text.RegularExpressions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.TestUtilities;
using FlaUI.UIA3;
using NUnit.Framework;


namespace ClinicManager.E2E.Tests.Core;

public abstract class UITestBase : FlaUITestBase
{
    static string ApplicationPath = string.Format("{0}\\..\\..\\..\\..\\Desktop\\ClinicManager.Win\\bin\\Debug\\ClinicMgr.exe",Environment.CurrentDirectory);

    static UITestBase()
    {
        NativeMethods.SetProcessDPIAware();
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        Mouse.MovePixelsPerMillisecond = 2;
        Retry.DefaultTimeout = TimeSpan.FromSeconds(5);
        Retry.DefaultInterval = TimeSpan.FromMilliseconds(250);
    }
    
    protected override AutomationBase GetAutomation()
        {
            return new UIA3Automation();
        }
        
    protected override Application StartApplication()
        {
            return FlaUI.Core.Application.Launch(ApplicationPath);
        }

    private static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();
    }
}
