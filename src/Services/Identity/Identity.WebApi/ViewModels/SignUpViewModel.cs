using System.ComponentModel.DataAnnotations;

namespace Identity.WebApi.Models;

public class SignUpViewModel
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    public required string UserName { get; set; }

    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
}
