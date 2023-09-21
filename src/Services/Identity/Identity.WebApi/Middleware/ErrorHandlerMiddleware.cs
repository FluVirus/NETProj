using Duende.IdentityServer.Validation;
using Identity.Domain.Exceptions;

namespace Identity.WebApi.Middleware;

public class ErrorHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    
    public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {       
        try
        {
            await next(context);
        }
        catch (Exception exception) 
        {
            HttpResponse response = context.Response;
            _logger.LogError(exception: exception, message: exception.Message, args: exception.Data);
            switch (exception)
            {
                case EmailNotFoundException:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            await response.WriteAsJsonAsync(new { errormessage = exception.Message });
        }
    }
}
