namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the SavingsProduct entity.
/// </summary>
internal sealed class SavingsProductConfiguration : IEntityTypeConfiguration<SavingsProduct>
{
    public void Configure(EntityTypeBuilder<SavingsProduct> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.InterestRate).HasPrecision(8, 4);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_SavingsProducts_Code");


        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_SavingsProducts_IsActive");
    }
}

