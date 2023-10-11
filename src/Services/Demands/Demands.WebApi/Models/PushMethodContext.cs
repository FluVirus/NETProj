using Demands.WebApi.Exceptions;

namespace Demands.WebApi.Models;

public sealed class PushMethodContext
{
    private CancellationTokenSource? _cts;

    private Func<CancellationToken, Task> _function;

    private Task? _task;

    private TimeSpan? _timeout;
    
    public PushMethodContext(Func<CancellationToken, Task> pushMethod)
    {
        _function = pushMethod;
    }

    public PushMethodContext(Func<CancellationToken, Task> pushMethod, TimeSpan timeout): this(pushMethod)
    {
        _timeout = timeout;
    }

    public void Start()
    {
        _cts = _timeout is null ? new() : new(_timeout.Value);
        _task = _function(_cts.Token);
    }

    public async Task StopAsync() 
    { 
        if (_task is null)
        {
            throw new PushMethodContextNotStartedException(message: $"[{_function.GetType().FullName} in PushMethodContext] Cannot stop method running when is has not been started");
        }

        _cts!.Cancel();
        await _task;
    }
}
