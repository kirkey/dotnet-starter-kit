namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the ShareProduct entity.
/// </summary>
internal sealed class ShareProductConfiguration : IEntityTypeConfiguration<ShareProduct>
{
    public void Configure(EntityTypeBuilder<ShareProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(64);
        
        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_ShareProducts_Code");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_ShareProducts_IsActive");
    }
}

