namespace Demands.WebApi.Exceptions;

public class PushMethodContextNotStartedException: InvalidOperationException
{
    public PushMethodContextNotStartedException(): base()
    {

    }

    public PushMethodContextNotStartedException(string? message): base(message) 
    { 
    
    } 
}
