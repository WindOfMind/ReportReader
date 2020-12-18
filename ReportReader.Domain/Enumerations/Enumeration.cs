namespace ReportReader.Domain.Enumerations
{
    internal abstract class Enumeration
    {
        protected Enumeration(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
