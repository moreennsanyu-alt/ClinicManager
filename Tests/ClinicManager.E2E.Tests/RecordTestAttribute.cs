using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.v3;

// Apply to every test in the assembly.
[assembly: ClinicManager.E2E.Tests.RecordTest]

namespace ClinicManager.E2E.Tests
{
    /// <summary>
    /// Records every test to "{TestResultsDirectory}/{MethodName}[_{theory args}].avi".
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class RecordTestAttribute : BeforeAfterTestAttribute
    {
        private const int MaxFileNameLength = 150;

        // Test results directory, used as the recordings directory.
        public static string TestResultsDirectory { get; set; } =
            Path.Combine(AppContext.BaseDirectory, "TestResults");

        // Attribute instances can be shared between tests, so state is keyed by test.
        private static readonly ConcurrentDictionary<string, TestScreenRecorder> Recorders = new();

        public override void Before(MethodInfo methodUnderTest, IXunitTest test)
        {
            try
            {
                var recorder = new TestScreenRecorder(TestResultsDirectory, BuildFileName(methodUnderTest, test));
                recorder.CreateRecording();
                Recorders[test.UniqueID] = recorder;
            }
            catch (Exception ex)
            {
                // Recording problems should not fail the test itself.
                Console.Error.WriteLine($"Could not start recording for {test.TestDisplayName}: {ex.Message}");
            }
        }

        public override void After(MethodInfo methodUnderTest, IXunitTest test)
        {
            if (!Recorders.TryRemove(test.UniqueID, out var recorder))
                return;

            try
            {
                recorder.EndRecording();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not stop recording for {test.TestDisplayName}: {ex.Message}");
            }
        }

        private static string BuildFileName(MethodInfo method, IXunitTest test)
        {
            string name = method.Name;

            // Theories: append the theory data of this row
            var args = test.TestMethodArguments;
            if (args is { Length: > 0 })
                name += "_" + string.Join("_", args.Select(a => a?.ToString() ?? "null"));

            // Sanitize invalid file name characters
            var invalid = Path.GetInvalidFileNameChars();
            name = new string(name.Select(c => invalid.Contains(c) || char.IsWhiteSpace(c) ? '_' : c).ToArray());

            // Keep paths short; add part of the unique ID so rows stay distinct
            if (name.Length > MaxFileNameLength)
                name = name[..MaxFileNameLength] + "_" + test.UniqueID[..8];

            return name + ".avi";
     
}
    }
}
