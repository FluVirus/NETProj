namespace Identity.WebApi.Models;

public class SignInModel
{
    public required string Email { get; set; }

    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }
}
