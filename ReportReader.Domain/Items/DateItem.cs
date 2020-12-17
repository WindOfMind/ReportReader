using System;
using System.Globalization;
using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class DateItem : Item
    {
        public DateTime? Value { get; }

        public DateItem(DateTime? value, Column column) : base(column)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value?.ToString("yyyy-mm-dd hh:mm:ss.sss", CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }
}