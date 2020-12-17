using System;
using System.Linq;

namespace ReportReader.App
{
    public class AppOptions
    {
        private const string ProjectOptionName = "--project";
        private const string FileOptionName = "--file";
        private const string SortByStartDateOptionName = "--sortByStartDate";

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
                Path = ReadValue(args, FileOptionName),
                SortByStartDate = args.Contains(SortByStartDateOptionName),
                ProjectId = ReadValue(args, ProjectOptionName)
            };
        }

        private static string ReadValue(string[] args, string name)
        {
            int namePosition = Array.IndexOf(args, name);
            int valuePosition = namePosition + 1;
            bool valueExists = valuePosition > 0 && valuePosition < args.Length;

            return valueExists
                ? args[valuePosition]
                : string.Empty;
        }
    }
}