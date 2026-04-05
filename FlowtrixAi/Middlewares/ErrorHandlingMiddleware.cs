using FlowtrixAI.Domain.Exceptions;

namespace FlowtrixAI.Api.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(ForbidenException forbiden)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync(forbiden.Message);
        }
        catch(NotFoundException notFound)
        {
            context.Response.StatusCode = 404;
            await  context.Response.WriteAsync(notFound.Message);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("SomeThing went wrong");
        }
    }
}
