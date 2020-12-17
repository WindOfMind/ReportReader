using System.IO;
using System.Threading.Tasks;
using ReportReader.Domain;
using ReportReader.Domain.Common;

namespace ReportReader.App
{
    public static class ReportService
    {
        public static async Task<Result<Report>> LoadFromFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Result<Report>($"File {filePath} not found.");
            }

            string[] reportText = await File.ReadAllLinesAsync(filePath);

            return Report.FromText(reportText);
        }
    }
}
