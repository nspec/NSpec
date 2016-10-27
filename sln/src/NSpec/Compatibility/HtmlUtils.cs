namespace NSpec.Compatibility
{
    public static class HtmlUtils
    {
        public static string Encode(string value)
        {
            string encoded;

#if NET452
            encoded = System.Web.HttpUtility.HtmlEncode(value);
#else
            encoded = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(value);
#endif

            return encoded;
        }
    }
}
