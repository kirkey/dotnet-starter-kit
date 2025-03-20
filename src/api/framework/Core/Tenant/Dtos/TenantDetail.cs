namespace FSH.Framework.Core.Tenant.Dtos;
public class TenantDetail
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime ValidUpto { get; set; }
    public string? Issuer { get; set; }
}
