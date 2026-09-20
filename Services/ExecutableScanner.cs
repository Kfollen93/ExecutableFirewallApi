using ExecutableFirewallApi.Models;

namespace ExecutableFirewallApi.Services
{
    public class ExecutableScanner : IExecutableScanner
    {
        private readonly HashSet<string> ExecutableExtensions =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".exe",
                ".com",
                ".scr"
            };

        private readonly EnumerationOptions enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
            ReturnSpecialDirectories = false
        };

        public IReadOnlyCollection<ExecutableInfo> Scan(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                return new List<ExecutableInfo>();
            }

            string[] programFiles = Directory
                .EnumerateFiles(folderPath, "*", enumerationOptions)
                .Where(file => ExecutableExtensions.Contains(Path.GetExtension(file)))
                .ToArray();

            var executableInfos = new List<ExecutableInfo>();

            foreach (string programFile in programFiles)
            {
                var executableInfo = new ExecutableInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    FileName = Path.GetFileName(programFile),
                    FullPath = programFile
                };

                executableInfos.Add(executableInfo);
            }

            return executableInfos;
        }
    }
}