namespace Common
{
    public static class UrlExtensions
    {
        public static bool IsVersion2(this string authority)
        {
            return !string.IsNullOrEmpty(authority) && authority.EndsWith("v2.0");
        }
    }
}
