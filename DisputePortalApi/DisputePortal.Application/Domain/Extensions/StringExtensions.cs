namespace DisputePortal.Application.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string ToDbTableName(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Contains("Model", StringComparison.InvariantCultureIgnoreCase)
                ? str.Replace("Model", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                : str;
        }
    }
}