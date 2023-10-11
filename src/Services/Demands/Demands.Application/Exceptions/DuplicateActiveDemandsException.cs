namespace Demands.Application.Exceptions;

public class DuplicateActiveDemandsException: Exception
{
    public DuplicateActiveDemandsException() : base(message: "Several active demands exist contemporaneously")
    { 
    
    }

    public DuplicateActiveDemandsException(string? message) : base(message) 
    { 
    
    }
}
