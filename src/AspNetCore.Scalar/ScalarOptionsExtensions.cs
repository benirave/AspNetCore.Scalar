using System.Text;

namespace AspNetCore.Scalar
{
    public static class ScalarOptionsExtensions
    {
        private const string DefaultCdnUrl = "https://cdn.jsdelivr.net/npm/@scalar/api-reference";

        
        public static void InjectStylesheet(this ScalarOptions options, string path, string media = "screen")
        {
            var builder = new StringBuilder(options.HeadContent);
            builder.AppendLine($"<link href='{path}' rel='stylesheet' media='{media}' type='text/css' />");
            options.HeadContent = builder.ToString();
        }
        
        public static void UseCdn(this ScalarOptions options, string url = DefaultCdnUrl)
        {
            options.CdnUrl = url;
        }

        public static void UseSpecUrl(this ScalarOptions options, string url)
        {
            options.SpecUrl = url;
        }

        public static void UseTheme(this ScalarOptions options, Theme theme)
        {
            options.ConfigObject.Theme = theme;
        }

        public static void UseLayout(this ScalarOptions options, Layout layout)
        {
            options.ConfigObject.Layout = layout;
        }

        public static void HideSidebar(this ScalarOptions options)
        {
            options.ConfigObject.ShowSidebar = false;
        }

        public static void UseSearchAsHotKey(this ScalarOptions options, char hotKey)
        {
            options.ConfigObject.SearchHotKey = hotKey;
        }

        public static void AddAdditionalItem(this ScalarOptions options, string key, object value)
        {
            options.ConfigObject.AdditionalItems.Add(key, value);
        }
    }
}