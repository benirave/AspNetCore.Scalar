using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace AspNetCore.Scalar
{
    public class ScalarOptions
    {
        public string RoutePrefix { get; set; } = "scalar-api-docs";

        public string DocumentTitle { get; set; } = "API Docs";

        public string HeadContent { get; set; } = string.Empty;

        public string SpecUrl { get; set; } = "../swagger/v1/swagger.json";
        
        public string ProxyUrl { get; set; } = string.Empty;

        public Func<Stream> IndexStream { get; set; } = () => typeof(ScalarOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("AspNetCore.Scalar.index.html");

        public ConfigObject ConfigObject { get; set; } = new ConfigObject();
    }

    public class ConfigObject
    {
        public Theme Theme { get; set; } = Theme.Default;

        public Layout Layout { get; set; } = Layout.Modern;

        public bool ShowSidebar { get; set; } = true;

        public char SearchHotKey { get; set; } = 'k';
        
        [JsonExtensionData]
        public Dictionary<string, object> AdditionalItems { get; set; } = new Dictionary<string, object>();
    }
}