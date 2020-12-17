using System;
using System.Globalization;
using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class DateItem : Item
    {
        private readonly string _dateTimeFormat;

        public DateTime? Value { get; }

        public DateItem(DateTime? value, Column column, string dateTimeFormat) : base(column)
        {
            _dateTimeFormat = dateTimeFormat;
            Value = value;
        }

        public override string ToString()
        {
            return Value?.ToString(_dateTimeFormat, CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }
}