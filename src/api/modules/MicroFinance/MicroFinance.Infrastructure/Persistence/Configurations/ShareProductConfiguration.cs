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
            .HasMaxLength(ShareProduct.CodeMaxLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ShareProduct.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(ShareProduct.DescriptionMaxLength);

        builder.Property(x => x.NominalValue).HasPrecision(18, 2);
        builder.Property(x => x.CurrentPrice).HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_ShareProducts_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_ShareProducts_Name");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_ShareProducts_IsActive");
    }
}
