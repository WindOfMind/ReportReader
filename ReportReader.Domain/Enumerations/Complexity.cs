namespace ReportReader.Domain.Enumerations
{
    internal class Complexity : Enumeration
    {
        public static readonly Complexity Simple = new("Simple");
        public static readonly Complexity Moderate = new("Moderate");
        public static readonly Complexity Hazardous = new("Hazardous");

        public static Complexity[] AllComplexities =
        {
            Simple,
            Moderate,
            Hazardous
        };

        private Complexity(string name) : base(name)
        {
        }
    }
}