using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Extensions;

internal static class IdentityManagersExtensions
{
    public static Exception ToException(this IdentityError error)
    {
        Exception exception = new Exception(message: $"[Code {error.Code}] {error.Description}");
        exception.Data["Code"] = error.Code;
        return exception;
    }

    public static IEnumerable<Exception> ToExceptions(this IEnumerable<IdentityError> errors)
    {
        return errors.Select(e => e.ToException());
    }

    public static AggregateException AsAggreateException(this IEnumerable<IdentityError> errors)
    {
        return new AggregateException(errors.ToExceptions());
    }

    public static void ThrowIfErrors(this IdentityResult result)
    {
        if (result.Succeeded)
        {
            return;
        }

        throw result.Errors.AsAggreateException();
    }
}
