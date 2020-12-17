namespace ReportReader.Domain.Enumerations
{
    internal abstract class Enumeration
    {
        public string Name { get; protected set; }

        protected Enumeration(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
