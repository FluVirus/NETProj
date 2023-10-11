using Demands.WebApi.Exceptions;
using Demands.WebApi.Models;

namespace Demands.WebApi.Extensions;

public static class ContextExtensions
{
    public static async Task DeletePushActionFromContext(this IDictionary<object, object?> hubCallerContext, object MethodContextKey)
    {
        bool result = hubCallerContext.TryGetValue(MethodContextKey, out var value);

        if (!result)
        {
            return;
        }

        if (value is not PushMethodContext pushMethodContext)
        {
            throw new ArgumentException(paramName: nameof(MethodContextKey), message: $"[ContextExtensions, ClearPushMethodContext] Cannot clear context of not PushMethodContext");
        }

        try
        {
            await pushMethodContext.StopAsync();
        }
        catch (PushMethodContextNotStartedException)
        {

        }

        hubCallerContext.Remove(MethodContextKey);
    }
}
