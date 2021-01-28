using ReportReader.Domain.Columns;
using ReportReader.Domain.Enumerations;

namespace ReportReader.Domain.Items
{
    internal class EnumerationItem : Item
    {
        public EnumerationItem(Enumeration value, Column column) : base(column)
        {
            Value = value;
        }

        public Enumeration Value { get; }

        public override string ToString()
        {
            return Value?.ToString() ?? string.Empty;
        }
    }
}