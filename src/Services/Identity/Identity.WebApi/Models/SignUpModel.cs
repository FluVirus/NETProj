namespace Identity.WebApi.Models;

public class SignUpModel
{
    public required string Email { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }
}
