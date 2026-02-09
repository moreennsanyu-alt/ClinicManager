using System;
using System.Text.RegularExpressions;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.TestUtilities;
using FlaUI.UIA3;
using NUnit.Framework;
using OperatingSystem = FlaUI.Core.Tools.OperatingSystem;
using Application = FlaUI.Core.Application;
using FlaUI.Core;

namespace ClinicManager.E2E.Tests.Core;

public class UITestBase: FlaUITestBase
{
    protected override AutomationBase GetAutomation()
    {
        return new UIA3Automation();
    }
    
    protected override FlaUI.Core.Application StartApplication()
        {
            if (OperatingSystem.IsWindows10())
            {
                // Use the store application on those systems
                return Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            }
            if (OperatingSystem.IsWindowsServer2016() || OperatingSystem.IsWindowsServer2019())
            {
                // The calc.exe on this system is just a stub which launches win32calc.exe
                return Application.Launch("win32calc.exe");
            }
            return Application.Launch("calc.exe");
        }
    }

