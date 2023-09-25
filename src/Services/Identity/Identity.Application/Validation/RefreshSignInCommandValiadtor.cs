using FluentValidation;
using Identity.Application.Refresh.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Validation;

public class RefreshSignInCommandValiadtor: AbstractValidator<RefreshSignInCommand>
{
    public RefreshSignInCommandValiadtor()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Continue;

        RuleFor(command => command.UserId).NotEmpty().GreaterThan(0);
    }
}
