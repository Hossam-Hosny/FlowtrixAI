using FlowtrixAI.Api.Middlewares;

namespace FlowtrixAI.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<ErrorHandlingMiddleware>();



    }
}
