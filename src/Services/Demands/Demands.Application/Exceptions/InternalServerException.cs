﻿namespace Demands.Application.Exceptions;

public class InternalServerException: Exception
{
    public InternalServerException(): base()
    {

    }

    public InternalServerException(string? message): base(message) 
    { 
    
    }
}
