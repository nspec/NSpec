namespace NSpec.Compatibility
{
    public static class HtmlUtils
    {
        public static string Encode(string value)
        {
            string encoded;

#if NETSTANDARD1_6
            encoded = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(value);
#else
            encoded = System.Web.HttpUtility.HtmlEncode(value);
#endif

            return encoded;
        }
    }
}
