namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the MfiConfiguration entity.
/// </summary>
internal sealed class MfiConfigurationConfiguration : IEntityTypeConfiguration<MfiConfiguration>
{
    public void Configure(EntityTypeBuilder<MfiConfiguration> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        // Indexes
        builder.HasIndex(x => x.BranchId);
    }
}