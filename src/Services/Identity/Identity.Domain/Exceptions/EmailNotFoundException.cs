using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Exceptions;

public class EmailNotFoundException: Exception
{ 
    public string? RequestedEmail { get; init; }

    public EmailNotFoundException() : base() { }

    public EmailNotFoundException(string message) : base(message) { }

    public EmailNotFoundException(string message, string? requestedEmail) : base(message: $"[Request: {requestedEmail ?? "unknown"}] {message}") 
    {
        RequestedEmail = requestedEmail;
    }
}
