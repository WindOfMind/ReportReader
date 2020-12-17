namespace ReportReader.Domain.Enumerations
{
    internal class Currency : Enumeration
    {
        public static readonly Currency Eur = new("EUR");

        public static Currency[] AllCurrencies =
        {
            Eur
        };

        private Currency(string name) : base(name)
        {
        }
    }
}