using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiVersioning.Config;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    public void Configure(string name, SwaggerGenOptions options) =>
        Configure(options);

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "API Teste (versionamento)",
            Version = description.ApiVersion.ToString(),
            Description = "Teste de versionamento de API com Swagger",
            Contact = new OpenApiContact { Name = "Author name", Email = "author-main@org.com" },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
        };

        if (description.IsDeprecated)
        {
            info.Description += " deprecated API.";
        }

        return info;
    }
}