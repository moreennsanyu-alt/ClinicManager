using NUnit.Framework;
using ClinicManager.E2E.Tests.Core;
using FlaUI.Core.Capturing;
using System.IO;
using System.Drawing.Imaging;

namespace ClinicManager.Application.FunctionalTests;

    [TestFixture]
    public class SampleTest
    {

        [Test]
        public void Test()
        {
            var window = Application.GetMainWindow(Automation
