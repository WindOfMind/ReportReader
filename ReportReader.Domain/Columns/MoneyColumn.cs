using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal class MoneyColumn : Column
    {
        public MoneyColumn(string name, bool allowNullValues = false) : base(name, allowNullValues)
        {
        }

        public override Result<Item> Parse(string value)
        {
            if (IsNullValue(value) && AllowNullValues)
            {
                return new Result<Item>(new MoneyItem(null, this));
            }

            if (!decimal.TryParse(value, out decimal money))
            {
                return new Result<Item>($"Unable to parse money value {value}.");
            }

            return new Result<Item>(new MoneyItem(money, this));
        }
    }
}