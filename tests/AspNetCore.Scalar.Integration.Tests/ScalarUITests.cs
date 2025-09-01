using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Scalar.Integration.Tests
{
    public class ScalarUITests
    {
        private static readonly ScalarOptions DefaultScalarOptions = new();

        private const string ScalarUIPath = "scalar-api-docs";
        private const string DefaultSwaggerDocPath = "swagger/v1/swagger.json";

        private static readonly IBrowsingContext HtmlParser = BrowsingContext.New(Configuration.Default);

        [Fact]
        public async Task UseScalar_ShouldRenderScalarUI()
        {
            // Arrange
            using var client = TestServerBuilder
                .BuildServer(DefaultScalarOptions)
                .CreateClient();

            // Act
            var swaggerDocResponse = await client.GetAsync(DefaultSwaggerDocPath);
            var healthResponse = await client.GetAsync("health");
            var scalarResponse = await client.GetAsync($"{ScalarUIPath}/index.html");

            // Assert
            Assert.True(healthResponse.IsSuccessStatusCode, "Api did not start");
            Assert.True(swaggerDocResponse.IsSuccessStatusCode, "Swagger doc did not load");
            Assert.True(scalarResponse.IsSuccessStatusCode, "Scalar UI did not render");

            var swaggerJson = await swaggerDocResponse.Content.ReadAsStringAsync();
            EnsureGivenEndpointExistsInSwaggerJson(swaggerJson, "/health");

            var scalarHtml = await scalarResponse.Content.ReadAsStringAsync();
            var scalarHtmlDocument = await HtmlParser.OpenAsync(req => req.Content(scalarHtml));
            var apiReferenceLocator = scalarHtmlDocument.GetElementById("api-reference");

            EnsureScalarHtmlDocumentContainsGivenSwaggerPath(apiReferenceLocator, DefaultScalarOptions.SpecUrl);
            
            EnsureScalarHtmlDocumentContainsGivenProxyPath(apiReferenceLocator, DefaultScalarOptions.ProxyUrl);

            EnsureScalarConfigurationIsCorrectlySetInHtmlDocument(apiReferenceLocator, DefaultScalarOptions);

            EnsureScalarStandaloneScriptIsSetInHtmlDocument(scalarHtmlDocument);
        }
        
        [Fact]
        public async Task UseScalarWithCdn_ShouldRenderScalarUI()
        {
            // Arrange
            const string DefaultScalarCdn = "https://cdn.jsdelivr.net/npm/@scalar/api-reference";
            
            var scalarOptions = new ScalarOptions();
            // ReSharper disable once RedundantArgumentDefaultValue
            scalarOptions.UseCdn(DefaultScalarCdn);
            
            using var client = TestServerBuilder
                .BuildServer(scalarOptions)
                .CreateClient();

            // Act
            var swaggerDocResponse = await client.GetAsync(DefaultSwaggerDocPath);
            var healthResponse = await client.GetAsync("health");
            var scalarResponse = await client.GetAsync($"{ScalarUIPath}/index.html");

            // Assert
            Assert.True(healthResponse.IsSuccessStatusCode, "Api did not start");
            Assert.True(swaggerDocResponse.IsSuccessStatusCode, "Swagger doc did not load");
            Assert.True(scalarResponse.IsSuccessStatusCode, "Scalar UI did not render");

            var swaggerJson = await swaggerDocResponse.Content.ReadAsStringAsync();
            EnsureGivenEndpointExistsInSwaggerJson(swaggerJson, "/health");

            var scalarHtml = await scalarResponse.Content.ReadAsStringAsync();
            var scalarHtmlDocument = await HtmlParser.OpenAsync(req => req.Content(scalarHtml));
            var apiReferenceLocator = scalarHtmlDocument.GetElementById("api-reference");

            EnsureScalarHtmlDocumentContainsGivenSwaggerPath(apiReferenceLocator, DefaultScalarOptions.SpecUrl);

            EnsureScalarConfigurationIsCorrectlySetInHtmlDocument(apiReferenceLocator, DefaultScalarOptions);

            EnsureGivenCdnIsUsed(scalarHtmlDocument, DefaultScalarCdn);
        }

        private static void EnsureGivenEndpointExistsInSwaggerJson(string swaggerJson, string endpointPath)
        {
            var doc = JsonSerializer.Deserialize<JsonElement>(swaggerJson);

            var paths = doc.GetProperty("paths");
            var endpointExists = paths.TryGetProperty(endpointPath, out _);

            Assert.True(endpointExists, nameof(EnsureGivenEndpointExistsInSwaggerJson));
        }

        private static void EnsureScalarHtmlDocumentContainsGivenSwaggerPath(IElement scalarElement, string swaggerJsonPath)
        {
            var dataUrl = scalarElement.GetAttribute("data-url");

            Assert.True(dataUrl.Equals(swaggerJsonPath, StringComparison.InvariantCultureIgnoreCase), nameof(EnsureScalarHtmlDocumentContainsGivenSwaggerPath));
        }
        
        private static void EnsureScalarHtmlDocumentContainsGivenProxyPath(IElement scalarElement, string expectedProxyUrl)
        {
            var proxyUrl = scalarElement.GetAttribute("data-proxy-url");

            Assert.True(proxyUrl.Equals(expectedProxyUrl, StringComparison.InvariantCultureIgnoreCase), nameof(EnsureScalarHtmlDocumentContainsGivenProxyPath));
        }
        
        private static void EnsureScalarConfigurationIsCorrectlySetInHtmlDocument(IElement scalarElement, ScalarOptions expectedScalarOption)
        {
            var configurationLocator = scalarElement.GetAttribute("data-configuration");
            var scalarConfiguration = JsonSerializer.Deserialize<ConfigObject>(configurationLocator);

            Assert.NotNull(scalarConfiguration);

            Assert.Equal(expectedScalarOption.ConfigObject.Theme, scalarConfiguration.Theme);
            Assert.Equal(expectedScalarOption.ConfigObject.Layout, scalarConfiguration.Layout);
            Assert.Equal(expectedScalarOption.ConfigObject.ShowSidebar, scalarConfiguration.ShowSidebar);
            Assert.Equal(expectedScalarOption.ConfigObject.SearchHotKey, scalarConfiguration.SearchHotKey);

            var configObjectPropertyNames = scalarConfiguration.GetType().GetProperties().Select(x => x.Name.ToLower());

            Assert.NotNull(scalarConfiguration.AdditionalItems);
            Assert.Equal(expectedScalarOption.ConfigObject.AdditionalItems, scalarConfiguration.AdditionalItems.Where(x => !configObjectPropertyNames.Contains(x.Key.ToLower())));
        }

        private static void EnsureScalarStandaloneScriptIsSetInHtmlDocument(IDocument htmlDocument)
        {
            var scriptTags = htmlDocument.GetElementsByTagName("script");
            var scriptsWithSrcAttribute = scriptTags
                .Select(x => x.GetAttribute("src"))
                .Where(x => !string.IsNullOrEmpty(x));

            Assert.Single(scriptsWithSrcAttribute);
            Assert.Contains("standalone.js", scriptsWithSrcAttribute);
        }
        
        private static void EnsureGivenCdnIsUsed(IDocument htmlDocument, string cdn)
        {
            var scriptTags = htmlDocument.GetElementsByTagName("script");
            var scriptsWithSrcAttribute = scriptTags
                .Select(x => x.GetAttribute("src"))
                .Where(x => !string.IsNullOrEmpty(x));

            Assert.Single(scriptsWithSrcAttribute);
            Assert.Contains(cdn, scriptsWithSrcAttribute);
        }
    }
}