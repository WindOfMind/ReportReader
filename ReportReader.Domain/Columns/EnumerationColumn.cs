using System.Collections.Generic;
using System.Linq;
using ReportReader.Domain.Common;
using ReportReader.Domain.Enumerations;
using ReportReader.Domain.Items;

namespace ReportReader.Domain.Columns
{
    internal class EnumerationColumn : Column
    {
        private readonly List<Enumeration> _allValues;

        public EnumerationColumn(string name, IEnumerable<Enumeration> allValues, bool allowNullValues = false) : base(name, allowNullValues)
        {
            _allValues = new List<Enumeration>(allValues);
        }

        public override Result<Item> Parse(string value)
        {
            if (IsNullValue(value) && AllowNullValues)
            {
                return new Result<Item>(new EnumerationItem(null, this));
            }

            var enumerationValue = _allValues.FirstOrDefault(e => e.Name == value);

            if (enumerationValue is null)
            {
                return new Result<Item>($"Unable to parse money value {value}.");
            }

            return new Result<Item>(new EnumerationItem(enumerationValue, this));
        }
    }
}