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

    public static int Main() => Execute<Build>(x => x.CodeCoverage);

    [Parameter("The solution configuration to build. Default is 'Debug' (local) or 'CI' (server).")]
    readonly Configuration Configuration = Configuration.Debug;

    [Parameter("Use this parameter if you encounter build problems in any way, " +
        "to generate a .binlog file which holds some useful information.")]
    readonly bool? GenerateBinLog;

    [Solution(GenerateProjects = true)]
    readonly Solution Solution;


    [Required]
    [GitRepository]
    readonly GitRepository GitRepository;

    AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";

    AbsolutePath AttachmentsDirectory => ArtifactsDirectory / "Attachments";

    AbsolutePath BuildLogsDirectory => AttachmentsDirectory / "build_logs";

    AbsolutePath CoverageDirectory => AttachmentsDirectory / "Coverage";

    AbsolutePath TestResultsDirectory => AttachmentsDirectory / "TestResults";

    string SemVer;

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

            DotNetToolRestore();
			DotNet("paket restore");
            
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
                    .SetBinaryLog(BuildLogsDirectory / $"ClinicManager.build.binlog")
                )
                .EnableNoLogo()
                .EnableNoRestore());
        });

    

    Project[] UnitTestProjects  => new[]{
         Solution.DesktopTests.ClinicManager_Win_Tests,
		 Solution.DesktopTests.ClinicManager_Tests,
    };

	Project[] E2ETestProjects  => new[]{
         Solution.DesktopTests.ClinicManager_E2E_Tests,
    };
   
    Target Tests => _ => _
        .DependsOn(UnitTests)
        .DependsOn(E2ETests);

    Target CodeCoverage => _ => _
        .DependsOn(Tests)
        .Executes(() =>
        {
            
string reportCommand = """
     reportgenerator \
      -reports:"./output/coverage/**/*.cobertura.xml" \
      -targetdir:"./output/test_results/coverage_reports" \
      -reporttypes:"lcov;HtmlInline_AzurePipelines_Dark" \
      -filefilters:"-*.g.cs;-*.nuget*" \
      -assemblyfilters:"+ClinicMgr"
    """;
DotNet(reportCommand);
            string link = TestResultsDirectory / "coverage_reports" / "index.html";
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
							"--coverage-output-format cobertura",
							$"--coverage-output {CoverageDirectory / $"{v.project.Name}_{v.framework}.cobertura.xml"}",
                            "--report-trx",
                            $"--report-trx-filename {v.project.Name}_{v.framework}.trx",
                            $"--results-directory {TestResultsDirectory}"
                        )
                    )
                );
        });

    Target E2ETests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var testCombinations =
                from project in E2ETestProjects
                let frameworks = project.GetTargetFrameworks()
                from framework in frameworks
                select new { project, framework };

                E2ETestProjects.ForEach(x=>Information(x.Name));

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
							"--coverage-output-format cobertura",
							$"--coverage-output {CoverageDirectory / $"{v.project.Name}_{v.framework}.cobertura.xml"}",
                            "--report-trx",
                            $"--report-trx-filename {v.project.Name}_{v.framework}.trx",
                            $"--results-directory {TestResultsDirectory}"
                         )
                    )
                );
        }
		);
    
    static bool IsDocumentation(string x) =>
        x.StartsWith("docs") ||
        x.StartsWith("CONTRIBUTING.md") ||
        x.StartsWith("cSpell.json") ||
        x.StartsWith("LICENSE") ||
        x.StartsWith("package.json") ||
        x.StartsWith("package-lock.json") ||
        x.StartsWith("README.md");
}
