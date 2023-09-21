using FluentValidation;
using Identity.WebApi.Models;

namespace Identity.WebApi.Validation;

public class SignInModelValidatior: AbstractValidator<SignInModel>
{
    public SignInModelValidatior()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(model => model.Email).NotEmpty().EmailAddress();
        RuleFor(model => model.Password).NotEmpty();
        RuleFor(model => model.ConfirmPassword).NotEmpty().Equal(model => model.Email);
    }
}
