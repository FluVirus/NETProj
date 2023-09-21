using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Identity.Application.Login.Commands;
using Identity.Application.Registration.Commands;
using Identity.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountsController: ControllerBase<AccountsController>
{
    public AccountsController
    (
        ILogger<AccountsController> logger,
        IMediator mediator,
        IMapper mapper
    ):
    base(logger, mediator, mapper)  { }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromForm] SignUpModel signUpModel, [FromServices] IValidator<SignUpModel> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(signUpModel);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        RegisterNewUserCommand command = _mapper.Map<RegisterNewUserCommand>(signUpModel);
        RegisterNewUserResult commandResult = await _mediator.Send(command);

        return commandResult.Status ? Ok() : throw new Exception("Internal Server Error");
    }

    public async Task<IActionResult> SignIn([FromForm] SignInModel signInModel, [FromServices] IValidator<SignInModel> validator)
    {
        ValidationResult validationResult = await validator.ValidateAsync(signInModel);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        LoginUserCommand command = _mapper.Map<LoginUserCommand>(signInModel);
        LoginUserResult commandResult = await _mediator.Send(command);

        return commandResult.Succeeded ? Ok() : throw new Exception("Internal Server Error");
    }

    public Task<IActionResult> SignOut()
    {
        //TODO: Handle SignOut
        throw new NotImplementedException();
    }
}
