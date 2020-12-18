using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal class StringColumn : Column
    {
        public StringColumn(string name, bool allowNullValues = false) : base(name, allowNullValues)
        {
        }

        public override Result<Item> Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new Result<Item>($"Value not found for column {Name}");
            }

            if (IsNullValue(value))
            {
                return AllowNullValues
                    ? new Result<Item>(new StringItem(string.Empty, this))
                    : new Result<Item>($"Value can not be NULL for column {Name} in the column {Name}");
            }

            return new Result<Item>(new StringItem(value, this));
        }
    }
}