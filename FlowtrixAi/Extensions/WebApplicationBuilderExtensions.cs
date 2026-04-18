using FlowtrixAI.Api.Middlewares;

namespace FlowtrixAI.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Flowtrix AI API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                In = Microsoft.OpenApi.ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(doc => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", doc),
                    new System.Collections.Generic.List<string>()
                }
            });
        });
        builder.Services.AddScoped<ErrorHandlingMiddleware>();



    }
}
