using FlowtrixAI.Api.Extensions;
using FlowtrixAI.Api.Middlewares;
using FlowtrixAI.Application.Extensions;
using FlowtrixAI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


// Add application , infrastructure and presentation services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.AddPresentation();



var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
