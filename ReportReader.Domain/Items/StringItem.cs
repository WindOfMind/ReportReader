using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class StringItem : Item
    {
        public StringItem(string value, Column column) : base(column)
        {
            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}