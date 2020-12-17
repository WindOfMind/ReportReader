using System;
using System.Threading.Tasks;

namespace ReportReader.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var options = AppOptions.FromArguments(args);

            if (string.IsNullOrEmpty(options.Path))
            {
                Console.WriteLine("Please specify a path for reading a file.");
                Console.WriteLine("Usage: ReportReaderApp --file <path>");

                return;
            }

            var reportResult = await ReportService.LoadFromFileAsync(options.Path);
            if (!reportResult.IsSuccessful)
            {
                Console.WriteLine("Error occurred during reading the file.");
                Console.WriteLine(reportResult.Error);

                return;
            }

            var report = reportResult.Value;

            if (!string.IsNullOrWhiteSpace(options.ProjectId))
            {
                report.FilterByProject(options.ProjectId);
            }

            if (options.SortByStartDate)
            {
                report.SortByStartDateAsc();
            }

            Console.WriteLine(report.ToString());
        }
    }
}
