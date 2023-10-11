namespace Demands.Application.Exceptions;

public class ProtocolException: InvalidOperationException
{
    public ProtocolException(): base()
    {

    }

    public ProtocolException(string? message) : base(message) 
    { 
    
    }
}
