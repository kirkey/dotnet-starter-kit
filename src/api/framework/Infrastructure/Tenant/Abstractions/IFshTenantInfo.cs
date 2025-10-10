namespace FSH.Framework.Infrastructure.Tenant.Abstractions;
public interface IFshTenantInfo : ITenantInfo
{
    string? ConnectionString { get; set; }
}
