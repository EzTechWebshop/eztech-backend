using EzTech.Data.Models;

namespace EzTech.Data.DtoModels;


public class WebsiteInfoDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string Cvr { get; set; } = null!;
    
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Facebook { get; set; } = null!;
    public string Instagram { get; set; } = null!;
    public string TrustPilot { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string FooterInfo { get; set; } = "";
    
    public List<WebsiteInfoTopic> WebsiteInfoFields { get; set; } = new ();
    
    public List<OpeningHours> WeeklyOpeningHours { get; set; } = new();
    public List<OpeningHoursSpecial> SpecialOpeningHours { get; set; } = new();
}

