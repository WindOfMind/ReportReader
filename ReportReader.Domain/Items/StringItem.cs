using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal class StringItem : Item
    {
        private readonly string _value;

        public string Value => _value;

        public StringItem(string value, Column column) : base(column)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}