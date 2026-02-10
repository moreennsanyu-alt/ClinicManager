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

    string AppPath = string.Format(@"{0}\..\..\ClinicManager.Win\Debug\ClinicMgr.exe", Environment.CurrentDirectory);
       
    protected override AutomationBase GetAutomation()
    {
        return new UIA3Automation();
    }
    
    protected override FlaUI.Core.Application StartApplication()
        => Application.Launch(AppPath);
            
        
    }

