using System.Globalization;
using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class MoneyItem : Item
    {
        public MoneyItem(decimal? value, Column column) : base(column)
        {
            Value = value;
        }

        public decimal? Value { get; }

        public override string ToString()
        {
            return Value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }
}