using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.ApiModels.UserApiModels.AuthApiModels;

public class SignInRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}