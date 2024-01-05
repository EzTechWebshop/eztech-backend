using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.ApiModels.UserApiModels.AuthApiModels;

public class SignUpRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not valid")]
    public string PhoneNumber { get; set; } = null!;
    
    public string? Address { get; set; } = "";
    public string? City { get; set; } = "";
    public string? Country { get; set; } = "";
    public string? PostalCode { get; set; } = "";
}