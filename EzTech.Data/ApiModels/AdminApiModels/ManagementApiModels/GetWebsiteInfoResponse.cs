using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetWebsiteInfoResponse
{
    public int AboutUsCount { get; set; }
    public int ContactUsCount { get; set; }
    public int TermsAndConditionsCount { get; set; }
    public int PrivacyPolicyCount { get; set; }
    public int ShippingPolicyCount { get; set; }
    public int ReturnPolicyCount { get; set; }
    public WebsiteInfoDto WebsiteInfo { get; set; } = null!;
    public string? Topic { get; set; }
    public List<WebsiteInfoTopic> WebsiteInfoFields { get; set; } = null!;
}