using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.ApiModels.UserApiModels;

public class UpdateUserPasswordRequest
{
    [Required]
    public string OldPassword { get; set; } = null!;
    [Required]
    public string NewPassword { get; set; } = null!;
}