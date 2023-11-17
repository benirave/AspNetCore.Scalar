using System.Text;

namespace AspNet.Scalar
{
    public static class ScalarOptionsExtensions
    {
        public static void InjectStylesheet(this ScalarOptions options, string path, string media = "screen")
        {
            var builder = new StringBuilder(options.HeadContent);
            builder.AppendLine($"<link href='{path}' rel='stylesheet' media='{media}' type='text/css' />");
            options.HeadContent = builder.ToString();
        }
        
        public static void UseSpecUrl(this ScalarOptions options, string url)
        {
            options.SpecUrl = url;
        }

        public static void UseTheme(this ScalarOptions options, Theme theme)
        {
            options.ConfigObject.Theme = theme;
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