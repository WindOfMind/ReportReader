using ReportReader.Domain.Common;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal abstract class Column
    {
        protected const string NullValue = "NULL";
        protected readonly bool AllowNullValues;

        protected Column(string name, bool allowNullValues)
        {
            Name = name;
            AllowNullValues = allowNullValues;
        }

        public string Name { get; protected set; }

        public abstract Result<Item> Parse(string value);

        public override string ToString()
        {
            return Name;
        }

        protected bool IsNullValue(string value) => value == NullValue;
    }
}