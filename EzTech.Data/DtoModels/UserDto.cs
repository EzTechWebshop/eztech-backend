namespace EzTech.Data.DtoModels;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; } = "";

    public string? Address { get; set; } = "";
    public string? City { get; set; } = "";
    public string? Country { get; set; } = "";
    public string? PostalCode { get; set; } = "";
}