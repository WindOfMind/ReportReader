using ReportReader.Domain.Columns;
using ReportReader.Domain.Enumerations;

namespace ReportReader.Domain.Items
{
    internal class EnumerationItem : Item
    {
        private readonly Enumeration _value;

        public EnumerationItem(Enumeration value, Column column) : base(column)
        {
            _value = value;
        }

        public Enumeration Value => _value;

        public override string ToString()
        {
            return _value?.ToString() ?? string.Empty;
        }
    }
}