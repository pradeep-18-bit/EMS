namespace EmployeeManagementSystem.Helpers
{
    public static class GeneratedFileStorage
    {
        public const string BasePath = "/app";
        public const string PayslipsFolder = "GeneratedPayslips";
        public const string LettersFolder = "GeneratedLetters";

        private static readonly char[] ExtraInvalidFileNameChars =
        {
            '<', '>', ':', '"', '/', '\\', '|', '?', '*'
        };

        public static string EnsureFolder(string folderName)
        {
            var fullPath = Path.Combine(BasePath, folderName);
            Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        public static string BuildRelativePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, SanitizeFileName(fileName))
                .Replace('\\', '/');
        }

        public static string GetFullPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new InvalidOperationException("Generated file path is empty.");

            var normalizedPath = relativePath.Replace('\\', '/');

            if (Path.IsPathRooted(normalizedPath) || normalizedPath.Contains(':'))
                throw new InvalidOperationException("Generated file path must be relative.");

            var basePath = Path.GetFullPath(BasePath);
            var fullPath = Path.GetFullPath(Path.Combine(BasePath, normalizedPath));
            var comparison = OperatingSystem.IsWindows()
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            if (!basePath.EndsWith(Path.DirectorySeparatorChar))
                basePath += Path.DirectorySeparatorChar;

            if (!fullPath.StartsWith(basePath, comparison))
                throw new InvalidOperationException("Generated file path points outside the storage folder.");

            return fullPath;
        }

        public static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars()
                .Concat(ExtraInvalidFileNameChars)
                .Distinct()
                .ToArray();

            var sanitized = new string(fileName
                .Select(ch => invalidChars.Contains(ch) ? '_' : ch)
                .ToArray())
                .Trim();

            return string.IsNullOrWhiteSpace(sanitized)
                ? $"generated-file-{Guid.NewGuid():N}"
                : sanitized;
        }
    }
}
