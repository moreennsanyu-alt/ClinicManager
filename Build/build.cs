using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.Xunit;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using Nuke.Components;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;
using static Nuke.Common.Tools.Xunit.XunitTasks;
using static Serilog.Log;
using Serilog;

[UnsetVisualStudioEnvironmentVariables]
[DotNetVerbosityMapping]
class Build : NukeBuild
{
    /* Support plugins are available for:
       - JetBrains ReSharper        https://nuke.build/resharper
       - JetBrains Rider            https://nuke.build/rider
       - Microsoft VisualStudio     https://nuke.build/visualstudio
       - Microsoft VSCode           https://nuke.build/vscode
    */

    public static int Main() => Execute<Build>(x => x.Tests);

    [Parameter("The solution configuration to build. Default is 'Debug' (local) or 'CI' (server).")]
    readonly Configuration Configuration = Configuration.Debug;

    [Parameter("Use this parameter if you encounter build problems in any way, " +
        "to generate a .binlog file which holds some useful information.")]
    readonly bool? GenerateBinLog;

    AbsolutePath BuildLogFile => TemporaryDirectory / "build.log";
    
    AbsolutePath FinalLogFile => ArtifactsDirectory / "build.log";

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;


    [Required]
    [GitRepository]
    readonly GitRepository GitRepository;

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";

    AbsolutePath TestResultsDirectory => RootDirectory / "TestResults";

    string SemVer;

    protected override void OnBuildInitialized()
    {
        base.OnBuildInitialized();

        // Write to Nuke's temporary directory to avoid conflicts with the Clean target
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console() 
            .WriteTo.File(BuildLogFile, rollingInterval: RollingInterval.Day) 
            .CreateLogger();
        
        Information($"Logging temporarily to: {BuildLogFile}");
    }

    protected override void OnBuildFinished()
    {
        base.OnBuildFinished();

        // 1. Flush and close the logger to release the file lock
        Log.CloseAndFlush();

        // 2. Copy the completed log to the Artifacts directory
        if (BuildLogFile.FileExists())
        {
            ArtifactsDirectory.CreateDirectory(); // Ensure directory exists
            File.Copy(BuildLogFile, FinalLogFile, overwrite: true);
        
            // Use standard Console.WriteLine here since Serilog is now closed
            Console.WriteLine($"Build log successfully saved to: {FinalLogFile}");
        }
    }

    Target Clean => _ => _
        .Executes(() =>
        {
            ArtifactsDirectory.CreateOrCleanDirectory();
            TestResultsDirectory.CreateOrCleanDirectory();
        });

    

    
    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            
            DotNetRestore(s => s
                .SetProjectFile(Solution)
                .EnableNoCache()
                //.SetConfigFile(RootDirectory / "nuget.config")
                );
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            ReportSummary(s => s
                .WhenNotNull(SemVer, (summary, semVer) => summary
                    .AddPair("Version", semVer)));

            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .When(_ => GenerateBinLog == true, c => c
                    .SetBinaryLog(ArtifactsDirectory / $"ClinicManager.build.binlog")
                )
                .EnableNoLogo()
                .EnableNoRestore());
        });

    

    Project[] UnitTestProjects  =>new[]{
         Source.Desktop_Tests.ClinicManager_Win_Tests,
    };

    IEnumerable<Project> E2ETestProjects => Solution.GetAllProjects("*")
            .Where(x => x.Name.EndsWith("E2E.Tests"));
            
    Target Tests => _ => _
        .DependsOn(UnitTests);
      //  .DependsOn(UnitTestsNet6OrGreater);

    Target CodeCoverage => _ => _
        .DependsOn(UnitTests)
        .Executes(() =>
        {
            ReportGenerator(s => s
                .SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll",
                    framework: "net10.0"))
                .SetTargetDirectory(TestResultsDirectory / "reports")
                .AddReports(TestResultsDirectory / "**/coverage.cobertura.xml")
                .AddReportTypes(
                    ReportTypes.lcov,
                    ReportTypes.HtmlInline_AzurePipelines_Dark)
                .AddFileFilters("-*.g.cs")
                .AddFileFilters("-*.nuget*")
                .SetAssemblyFilters("+ClinicManager"));

            string link = TestResultsDirectory / "reports" / "index.html";
            Information($"Code coverage report: \x1b]8;;file://{link.Replace('\\', '/')}\x1b\\{link}\x1b]8;;\x1b\\");
        });

    

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testCombinations =
                from project in UnitTestProjects
                let frameworks = project.GetTargetFrameworks()
                from framework in frameworks
                select new { project, framework };

                UnitTestProjects.ForEach(x=>Information(x.Name));

            DotNetRun(s => s
                .SetConfiguration(Configuration.Debug)
                .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                .EnableNoBuild()
                .CombineWith(
                    testCombinations,
                    (settings, v) => settings
                        .SetProjectFile(v.project)
                        .SetFramework(v.framework)
                        .SetProcessAdditionalArguments(
                            "--",
                            "--coverage",
                            "--report-trx",
                            $"--report-trx-filename {v.project.Name}_{v.framework}.trx",
                            $"--results-directory {TestResultsDirectory}"
                        )
                    )
                );
        });

        
    
    static bool IsDocumentation(string x) =>
        x.StartsWith("docs") ||
        x.StartsWith("CONTRIBUTING.md") ||
        x.StartsWith("cSpell.json") ||
        x.StartsWith("LICENSE") ||
        x.StartsWith("package.json") ||
        x.StartsWith("package-lock.json") ||
        x.StartsWith("README.md");
}
