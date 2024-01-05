using System.ComponentModel.DataAnnotations;
using EzTech.Data.ApiModels.FaqApiModels;
using EzTech.Data.ApiModels.WebsiteInfoApiModels;
using EzTech.Data.DtoModels;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Data.Models;

public class WebsiteInfo
{
    public int Id { get; set; }

    // Formal information
    [Required] 
    public string Name { get; set; } = null!;
    [Required] 
    public string CompanyName { get; set; } = null!;
    [Required] 
    public string Cvr { get; set; } = null!;

    [Required] 
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Country { get; set; } = null!;
    [Required] 
    public string PhoneNumber { get; set; } = null!;
    [Required] 
    public string Email { get; set; } = null!;

    // Social media
    [Required] 
    public string Facebook { get; set; } = null!;
    [Required] 
    public string Instagram { get; set; } = null!;
    [Required] 
    public string TrustPilot { get; set; } = null!;
    [Required] 
    public string Website { get; set; } = null!;

    /// <summary>
    /// The fields that are shown on the website about us page
    /// </summary>
    [Required]
    public List<WebsiteInfoText> WebsiteInfoFields { get; set; } = new();

    /// <summary>
    /// Will be shown on the website if there is a message that needs to be shown
    /// </summary>
    public string? WebsiteMessage { get; set; }

    [Required] 
    public string FooterInfo { get; set; } = null!;

    // Opening hours
    // I am not sure how to do this, so will be done like this for now
    public List<OpeningHours> WeeklyOpeningHours { get; set; } = new();
    public List<OpeningHoursSpecial> SpecialOpeningHours { get; set; } = new();

    public void Update(UpdateWebsiteInfoRequest request)
    {
        Name = request.Name ?? Name;
        CompanyName = request.CompanyName ?? CompanyName;
        Cvr = request.Cvr ?? Cvr;
        City = request.City ?? City;
        PostalCode = request.PostalCode ?? PostalCode;
        Address = request.Address ?? Address;
        Country = request.Country ?? Country;
        PhoneNumber = request.PhoneNumber ?? PhoneNumber;

        Email = request.Email ?? Email;
        Facebook = request.Facebook ?? Facebook;
        Instagram = request.Instagram ?? Instagram;
        TrustPilot = request.TrustPilot ?? TrustPilot;
        Website = request.Website ?? Website;
        FooterInfo = request.FooterInfo ?? FooterInfo;
    }
}