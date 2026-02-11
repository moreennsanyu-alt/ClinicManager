using NUnit.Framework;
using ClinicManager.E2E.Tests.Core;

namespace ClinicManager.E2E.Tests;

    [TestFixture]
    public class SampleTest : UITestBase
    {

        [Test]
        public void Test()
        {
            var window = Application.GetMainWindow(Automation);

            using (var image = Capture.Element(window)) 
        {
            // 2. Define a unique file path for the screenshot
            // TestContext.CurrentContext.WorkDirectory puts it in the bin/Debug folder usually
            string fileName = $"Screenshot_{TestContext.CurrentContext.Test.Name}.png";
            string filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);

            // 3. Save the Bitmap to disk
            image.Bitmap.Save(filePath, ImageFormat.Png);

            // 4. Attach the file to the NUnit Test Context
            // This ensures it shows up in your .trx report or CI pipeline
            TestContext.AddTestAttachment(filePath, "Main Window Snapshot");
        }
        }

    }
