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
            .HasMaxLength(64);

        builder.Property(x => x.FeeType)
            .HasMaxLength(64);


        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("IX_FeeDefinitions_Code");

        builder.HasIndex(x => x.FeeType)
            .HasDatabaseName("IX_FeeDefinitions_FeeType");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_FeeDefinitions_IsActive");
    }
}

