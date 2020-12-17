using System;
using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal class DateColumn : Column
    {
        public DateColumn(string name, bool allowNullValues = false) : base(name, allowNullValues)
        {
        }

        public override Result<Item> Parse(string value)
        {
            if (IsNullValue(value) && AllowNullValues)
            {
                new Result<Item>(new DateItem(null, this));
            }

            if (!DateTime.TryParse(value, out DateTime dateTime))
            {
                return new Result<Item>($"Unable to parse date {value}.");
            }

            return new Result<Item>(new DateItem(dateTime, this));
        }
    }
}