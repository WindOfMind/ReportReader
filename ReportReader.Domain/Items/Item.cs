using ReportReader.Domain.Columns;

namespace ReportReader.Domain.Items
{
    internal abstract class Item
    {
        public Column Column { get; }

        protected Item(Column column)
        {
            Column = column;
        }

        public abstract override string ToString();
    }
}