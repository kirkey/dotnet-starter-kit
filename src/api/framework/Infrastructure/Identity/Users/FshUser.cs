using Microsoft.AspNetCore.Identity;

namespace FSH.Framework.Infrastructure.Identity.Users;
public class FshUser : IdentityUser
{
    public DefaultIdType? EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Uri? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }
    public DateTime? LastLoginDateTime { get; set; }
    public string? LastLoginIp { get; set; }
    public string? LastLoginDeviceType { get; set; }
    public string? LastLoginLocation { get; set; }
}