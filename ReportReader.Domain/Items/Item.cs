using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal abstract class Item
    {
        protected Item(Column column)
        {
            Column = column;
        }

        public Column Column { get; }

        public abstract override string ToString();
    }
}