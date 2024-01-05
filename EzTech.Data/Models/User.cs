using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EzTech.Data.ApiModels.UserApiModels;

namespace EzTech.Data.Models;

public class User
{

    public void UpdateDetails(UpdateUserDetailsRequest request)
    {
        FirstName = request.FirstName ?? FirstName;
        LastName = request.LastName ?? LastName;
        Email = request.Email ?? Email;
        PhoneNumber = request.PhoneNumber ?? PhoneNumber;
        Address = request.Address ?? Address;
        City = request.City ?? City;
        Country = request.Country ?? Country;
        PostalCode = request.PostalCode ?? PostalCode;
        
    }
    
    [Key]
    public int Id { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(500)]
    public string Email { get; set; } = null!;
    [Required]
    public string Role { get; set; } = UserRoles.User;
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    public bool IsEmailVerified { get; set; } = false;
    public string VerificationToken { get; set; } = null!;
    public string? PhoneNumber { get; set; } = "";  // TODO: Maybe do phone number validation later, until then this is optional
    public string? Address { get; set; } = "";
    public string? City { get; set; } = "";
    public string? Country { get; set; } = "";
    public string? PostalCode { get; set; } = "";
    
    public Cart Cart { get; set; } = new();
    public Wishlist Wishlist { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<Rating> Ratings { get; set; } = new();
    
    [Required]
    [JsonIgnore]
    public string Salt { get; set; } = null!;
    [Required]
    [JsonIgnore]
    public string Hash { get; set; } = null!;
}

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string User = "User";
}
