using System.Diagnostics;

namespace EmployeeManagementSystem.Helpers
{
    public static class LibreOfficeHelper
    {
        private static readonly string[] KnownExecutablePaths =
        {
            @"C:\Program Files\LibreOffice\program\soffice.exe",
            @"C:\Program Files (x86)\LibreOffice\program\soffice.exe",
            "/usr/bin/soffice",
            "/usr/local/bin/soffice",
            "/usr/lib/libreoffice/program/soffice"
        };

        public static void ConvertToPdf(string inputPath, string outputPath)
        {
            var outputDirectory = Path.GetDirectoryName(outputPath)
                ?? throw new InvalidOperationException("Unable to resolve the PDF output directory.");

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ResolveExecutablePath(),
                    Arguments =
                        $"--headless --convert-to pdf --outdir \"{outputDirectory}\" \"{inputPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            var standardError = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"LibreOffice conversion failed with exit code {process.ExitCode}: {standardError}");
            }

            if (!File.Exists(outputPath))
            {
                throw new FileNotFoundException(
                    "LibreOffice did not create the expected PDF output.",
                    outputPath);
            }
        }

        private static string ResolveExecutablePath()
        {
            var configuredPath = Environment.GetEnvironmentVariable("LIBREOFFICE_PATH");

            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                return configuredPath;
            }

            foreach (var candidatePath in KnownExecutablePaths)
            {
                if (File.Exists(candidatePath))
                {
                    return candidatePath;
                }
            }

            return "soffice";
        }
    }
}
