using Finbuckle.MultiTenant.EntityFrameworkCore;

namespace FSH.Framework.Infrastructure.Identity.Persistence;
public class IdentityDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<IdentityDbContext> options, IOptions<DatabaseOptions> settings) 
    : MultiTenantIdentityDbContext<FshUser,
        FshRole,
        string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        FshRoleClaim,
        IdentityUserToken<string>>(multiTenantContextAccessor, options)
{
    private readonly DatabaseOptions _settings = settings.Value;
    private new FshTenantInfo TenantInfo { get; set; } = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;

    public DbSet<AuditTrail> AuditTrails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.ConfigureDatabase(_settings.Provider, TenantInfo.ConnectionString);
        }
    }
}
