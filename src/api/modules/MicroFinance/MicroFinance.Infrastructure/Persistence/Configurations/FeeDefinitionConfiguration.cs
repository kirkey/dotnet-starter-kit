namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FeeDefinition entity.
/// </summary>
internal sealed class FeeDefinitionConfiguration : IEntityTypeConfiguration<FeeDefinition>
{
    public void Configure(EntityTypeBuilder<FeeDefinition> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(FeeDefinition.CodeMaxLength);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(FeeDefinition.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(FeeDefinition.DescriptionMaxLength);

        builder.Property(x => x.FeeType)
            .IsRequired()
            .HasMaxLength(FeeDefinition.FeeTypeMaxLength);

        builder.Property(x => x.CalculationType)
            .IsRequired()
            .HasMaxLength(FeeDefinition.CalculationTypeMaxLength);

        builder.Property(x => x.AppliesTo)
            .IsRequired()
            .HasMaxLength(FeeDefinition.AppliesToMaxLength);

        builder.Property(x => x.ChargeFrequency)
            .IsRequired()
            .HasMaxLength(FeeDefinition.ChargeFrequencyMaxLength);

        builder.Property(x => x.Amount).HasPrecision(18, 4);
        builder.Property(x => x.MinAmount).HasPrecision(18, 2);
        builder.Property(x => x.MaxAmount).HasPrecision(18, 2);
        builder.Property(x => x.TaxRate).HasPrecision(5, 2);

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_FeeDefinitions_Code");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_FeeDefinitions_Name");

        builder.HasIndex(x => x.FeeType)
            .HasDatabaseName("IX_FeeDefinitions_FeeType");

        builder.HasIndex(x => x.AppliesTo)
            .HasDatabaseName("IX_FeeDefinitions_AppliesTo");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_FeeDefinitions_IsActive");
    }
}
