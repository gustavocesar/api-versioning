using ApiVersioning.Config;
using Asp.Versioning;
using Asp.Versioning.Builder;

namespace ApiVersioning.Extensions;

public static class ApiExtension
{
    public static void AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }

    public static void UseMinimalEndpoints(this WebApplication app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        app.MapGet("api/v{version:apiVersion}", async () =>
        {
            return await Task.Run(() => "Ok minimal v1");
        })
        .WithApiVersionSet(apiVersionSet)
        // .MapToApiVersion(1)
        ;

        app.MapGet("api/v{version:apiVersion}", async () =>
        {
            return await Task.Run(() => "Ok minimal v2");
        })
        .WithApiVersionSet(apiVersionSet)
        .MapToApiVersion(2);
    }

    public static void UseNormalEndpoints(this WebApplication app)
    {
        app.MapControllers();
    }

    public static void ConfigureSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions();

                // Build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }
}