namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FeeCharge entity.
/// </summary>
internal sealed class FeeChargeConfiguration : IEntityTypeConfiguration<FeeCharge>
{
    public void Configure(EntityTypeBuilder<FeeCharge> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(32);

        builder.Property(x => x.Amount).HasPrecision(18, 2);

        // Relationships
        builder.HasOne<FeeDefinition>()
            .WithMany()
            .HasForeignKey(x => x.FeeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.FeeDefinitionId)
            .HasDatabaseName("IX_FeeCharges_FeeDefinitionId");

        builder.HasIndex(x => x.LoanId)
            .HasDatabaseName("IX_FeeCharges_LoanId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FeeCharges_Status");
    }
}

