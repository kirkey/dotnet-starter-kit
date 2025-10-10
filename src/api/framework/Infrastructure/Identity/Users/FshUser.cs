using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.Framework.Infrastructure.Identity.Users;
public class FshUser : IdentityUser
{
    public DefaultIdType? EmployeeId { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? FirstName { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? LastName { get; set; }
    public Uri? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    [Column(TypeName = "VARCHAR(128)")]
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? ObjectId { get; set; }
    public DateTime? LastLoginDateTime { get; set; }
    [Column(TypeName = "VARCHAR(16)")]
    public string? LastLoginIp { get; set; }
    [Column(TypeName = "VARCHAR(64)")]
    public string? LastLoginDeviceType { get; set; }
    [Column(TypeName = "VARCHAR(1024)")]
    public string? LastLoginLocation { get; set; }
}
