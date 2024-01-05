using EzTech.Data.ApiModels.WebsiteInfoApiModels;

namespace EzTech.Data.Models;

public class WebsiteInfoText
{
    public void Update(UpdateWebsiteInfoTextRequest request)
    {
        Title = request.Title ?? Title;
        Description = request.Description ?? Description;
    }

    public int Id { get; set; }
    public WebsiteInfoFieldType Type { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}

public enum WebsiteInfoFieldType
{
    AboutUs,
    ContactUs,
    TermsAndConditions,
    PrivacyPolicy,
    ShippingPolicy,
    ReturnPolicy
}