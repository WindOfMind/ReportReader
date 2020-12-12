using System;

namespace ReportReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = AppOptions.FromArguments(args);

            if (string.IsNullOrEmpty(options.Path))
            {
                Console.WriteLine("Please specify a path for reading a report.");
                Console.WriteLine("Usage: ReportReaderApp --file <path>");

                return;
            }
        }
    }
}
