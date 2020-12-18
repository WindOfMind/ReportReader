using System;
using System.Globalization;
using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal class DateColumn : Column
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public DateColumn(string name, bool allowNullValues = false) : base(name, allowNullValues)
        {
        }

        public override Result<Item> Parse(string value)
        {
            if (IsNullValue(value) && AllowNullValues)
            {
                return new Result<Item>(new DateItem(null, this, DateTimeFormat));
            }

            if (!DateTime.TryParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return new Result<Item>($"Failed to parse date {value}");
            }

            return new Result<Item>(new DateItem(dateTime, this, DateTimeFormat));
        }
    }
}