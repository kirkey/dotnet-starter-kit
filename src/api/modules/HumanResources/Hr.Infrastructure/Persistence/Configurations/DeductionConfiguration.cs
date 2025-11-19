using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for Deduction entity.
/// </summary>
internal sealed class DeductionConfiguration : IEntityTypeConfiguration<Deduction>
{
    public void Configure(EntityTypeBuilder<Deduction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DeductionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.DeductionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.RecoveryMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.RecoveryFixedAmount)
            .HasPrecision(18, 2);

        builder.Property(d => d.RecoveryPercentage)
            .HasPrecision(5, 2);

        builder.Property(d => d.MaxRecoveryPercentage)
            .HasPrecision(5, 2);

        builder.Property(d => d.GlAccountCode)
            .HasMaxLength(20);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.HasIndex(d => d.DeductionName)
            .HasDatabaseName("IX_Deduction_DeductionName");

        builder.HasIndex(d => d.DeductionType)
            .HasDatabaseName("IX_Deduction_DeductionType");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_Deduction_IsActive");

        builder.HasIndex(d => new { d.DeductionType, d.IsActive })
            .HasDatabaseName("IX_Deduction_Type_Active");

        // Optimized for active deduction browsing
        builder.HasIndex(d => new { d.IsActive, d.DeductionType })
            .HasDatabaseName("IX_Deduction_Active_Type");

        // Recovery method queries
        builder.HasIndex(d => new { d.RecoveryMethod, d.IsActive })
            .HasDatabaseName("IX_Deduction_RecoveryMethod_Active");

        // GL account reconciliation
        builder.HasIndex(d => new { d.GlAccountCode, d.DeductionType, d.IsActive })
            .HasDatabaseName("IX_Deduction_GlAccount_Type_Active");
    }
}

