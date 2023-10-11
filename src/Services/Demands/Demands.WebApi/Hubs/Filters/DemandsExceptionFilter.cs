using Microsoft.AspNetCore.SignalR;

namespace Demands.WebApi.Hubs.Filters;

public class DemandsExceptionFilter: IHubFilter
{
    private readonly ILogger<DemandsExceptionFilter> _logger;

    public DemandsExceptionFilter(ILogger<DemandsExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object?>> next)
    {
        try
        {
            return await next(invocationContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(message: 
                $"{DateTime.Now.ToString("G")} {ex.Message}{Environment.NewLine}" +
                $"Hub method:{invocationContext.HubMethodName}, " +
                $"arguments: {string.Join(" ", invocationContext.HubMethodArguments.Select(arg => arg is not null? arg.ToString() : "null"))}");
            _logger.LogTrace(message: $"{ex.StackTrace}");

            throw new HubException(ex.Message);
        }
    }
}
