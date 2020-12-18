namespace ReportReader.Domain.Common
{
    public class Result<T>
    {
        public Result(T value)
        {
            Value = value;
            IsSuccessful = true;
        }

        public Result(string error)
        {
            Error = error;
            IsSuccessful = false;
        }

        public bool IsSuccessful { get; }

        public T Value { get; }

        public string Error { get; } = string.Empty;
    }
}