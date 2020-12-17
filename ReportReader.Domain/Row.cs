using System;
using System.Collections.Generic;
using System.Linq;
using ReportReader.Domain.Columns;
using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain
{
    internal class Row
    {
        private readonly List<Item> _items;

        private Row(IEnumerable<Item> items)
        {
            _items = new List<Item>(items);
        }

        public T GetItemByColumn<T>(Column column) where T : Item
        {
            return _items.Single(i => i.Column == column) as T;
        }

        public static Result<Row> FromLine(string reportLine, IReadOnlyCollection<Column> columns, char columnSeparator)
        {
            var values = reportLine.Split(columnSeparator).ToArray();
            var items = new List<Item>(columns.Count);

            if (values.Length != columns.Count)
            {
                string error = $"The next line:{Environment.NewLine}{reportLine}{Environment.NewLine}Does not contain all values";
                return new Result<Row>(error);
            }

            for (int columnNumber = 0; columnNumber < columns.Count; columnNumber++)
            {
                var column = columns.ElementAt(columnNumber);
                Result<Item> result = column.Parse(values[columnNumber]);

                if (!result.IsSuccessful)
                {
                    string error = $"Error happened in the next line:{Environment.NewLine}{reportLine}{Environment.NewLine}{result.Error}";
                    return new Result<Row>(error);
                }

                items.Add(result.Value);
            }

            return new Result<Row>(new Row(items));
        }

        public override string ToString()
        {
            return string.Join(" ", _items.Where(i => !string.IsNullOrEmpty(i.ToString())));
        }
    }
}