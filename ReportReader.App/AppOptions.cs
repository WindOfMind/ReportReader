using System;
using System.Linq;

namespace ReportReader.App
{
    public class AppOptions
    {
        public string Path { get; protected set; }

        public bool SortByStartDate { get; protected set; }

        public string ProjectId { get; protected set; }

        private AppOptions()
        {
        }

        public static AppOptions FromArguments(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            return new AppOptions
            {
                Path = ReadValue(args, "--file"),
                SortByStartDate = args.Contains("--sortByStartDate"),
                ProjectId = ReadValue(args, "--project")
            };
        }

        private static string ReadValue(string[] args, string name)
        {
            int namePosition = Array.IndexOf(args, name);
            int valuePosition = namePosition + 1;
            bool valueExists = valuePosition > 0 && valuePosition < args.Length;

            string value = valueExists
                ? args[valuePosition]
                : string.Empty;

            if (value.StartsWith("--"))
                return string.Empty;

            return value;
        }
    }
}