using System.Globalization;
using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class MoneyItem : Item
    {
        private readonly decimal? _value;

        public MoneyItem(decimal? value, Column column) : base(column)
        {
            _value = value;
        }

        public decimal? Value => _value;

        public override string ToString()
        {
            return _value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }
}