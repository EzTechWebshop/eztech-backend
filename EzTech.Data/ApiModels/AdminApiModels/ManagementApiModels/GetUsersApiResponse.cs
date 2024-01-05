using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetUsersApiResponse
{
    public int TotalUsers { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public List<UserDto> Users { get; set; } = null!;
    public UserDto? User { get; set; }
}