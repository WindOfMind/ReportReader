using System;
using System.Linq;

namespace ReportReaderApp
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
            int position = Array.IndexOf(args, name);
            bool canReadValue = position + 1 > args.Length - 1;

            string value = position >= 0 && canReadValue
                ? args[position + 1]
                : string.Empty;

            if (value.StartsWith("--"))
                return string.Empty;

            return value;
        }
    }
}