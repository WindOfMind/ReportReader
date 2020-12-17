using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportReader.Domain.Columns;
using ReportReader.Domain.Common;
using ReportReader.Domain.Enumerations;
using ReportReader.Domain.Items;

namespace ReportReader.Domain
{
    public class Report
    {
        private const string ProjectColumnName = "Project";
        private const string DescriptionColumnName = "Description";
        private const string StartDateColumnName = "Start date";
        private const string CategoryColumnName = "Category";
        private const string ResponsibleColumnName = "Responsible";
        private const string SavingsAmountColumnName = "Savings amount";
        private const string CurrencyColumnName = "Currency";
        private const string ComplexityColumnName = "Complexity";

        private const string CommentSymbol = "#";
        private const char ColumnSeparator = '\t';

        private static readonly Dictionary<string, Column> AllColumns = new()
        {
            { ProjectColumnName, new StringColumn(ProjectColumnName) },
            { DescriptionColumnName, new StringColumn(DescriptionColumnName) },
            { StartDateColumnName, new DateColumn(StartDateColumnName) },
            { CategoryColumnName, new StringColumn(CategoryColumnName) },
            { ResponsibleColumnName, new StringColumn(ResponsibleColumnName) },
            { SavingsAmountColumnName, new MoneyColumn(SavingsAmountColumnName, allowNullValues: true) },
            { CurrencyColumnName, new EnumerationColumn(CurrencyColumnName, Currency.AllCurrencies, allowNullValues: true) },
            { ComplexityColumnName, new EnumerationColumn(ComplexityColumnName, Complexity.AllComplexities) }
        };

        private readonly Header _header;
        private List<Row> _rows;

        private Report(Header header, IEnumerable<Row> rows)
        {
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _rows = rows?.ToList() ?? throw new ArgumentNullException(nameof(rows));
        }

        public void FilterByProject(string projectId)
        {
            if (string.IsNullOrWhiteSpace(projectId))
                throw new ArgumentException("Project id can be null or empty", nameof(projectId));

            Column projectColumn = AllColumns[ProjectColumnName];

            _rows = _rows.Where(r => r.GetItemByColumn<StringItem>(projectColumn).Value == projectId)
                .ToList();
        }

        public void SortByStartDateAsc()
        {
            var startDateColumn = AllColumns[StartDateColumnName];

            _rows.Sort((row1, row2) =>
            {
                var date1 = row1.GetItemByColumn<DateItem>(startDateColumn).Value;
                var date2 = row2.GetItemByColumn<DateItem>(startDateColumn).Value;

                return Nullable.Compare(date1, date2);
            });
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(_header.ToString());

            foreach (var row in _rows)
            {
                sb.AppendLine(row.ToString());
            }

            return sb.ToString();
        }

        public static Result<Report> FromText(string[] textLines)
        {
            if (textLines == null)
                throw new ArgumentNullException(nameof(textLines));

            Result<Header> headerResult = ReadHeader(textLines, out int headerPosition);
            if (!headerResult.IsSuccessful)
            {
                return new Result<Report>(headerResult.Error);
            }
            Header header = headerResult.Value;

            Result<List<Row>> rows = ReadRows(textLines.Skip(headerPosition + 1), header.Columns);
            if (!rows.IsSuccessful)
            {
                return new Result<Report>(rows.Error);
            }

            return new Result<Report>(new Report(header, rows.Value));
        }

        private static Result<Header> ReadHeader(string[] reportLines, out int headerPosition)
        {
            headerPosition = 0;
            for (int i = 0; i < reportLines.Length; i++)
            {
                var line = reportLines[i];

                if (IsLineToSkip(line))
                {
                    continue;
                }

                headerPosition = i;
                return Header.FromString(line, AllColumns, ColumnSeparator);
            }

            return new Result<Header>("Failed to read a header. Please, check that the report has the correct header.");
        }

        private static Result<List<Row>> ReadRows(IEnumerable<string> reportBody, IReadOnlyCollection<Column> columns)
        {
            var rows = new List<Row>();
            foreach (string line in reportBody)
            {
                if (IsLineToSkip(line))
                    continue;

                Result<Row> row = Row.FromLine(line, columns, ColumnSeparator);

                if (!row.IsSuccessful)
                {
                    return new Result<List<Row>>(row.Error);
                }

                rows.Add(row.Value);
            }

            return new Result<List<Row>>(rows);
        }

        private static bool IsLineToSkip(string line)
        {
            return line.StartsWith(CommentSymbol) || string.IsNullOrWhiteSpace(line);
        }
    }
}
