using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PayComponentConfiguration : IEntityTypeConfiguration<PayComponent>
{
    public void Configure(EntityTypeBuilder<PayComponent> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ComponentName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ComponentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CalculationMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CalculationFormula)
            .HasMaxLength(500);

        builder.Property(x => x.Rate)
            .HasPrecision(18, 6);

        builder.Property(x => x.FixedAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.MinValue)
            .HasPrecision(15, 2);

        builder.Property(x => x.MaxValue)
            .HasPrecision(15, 2);

        builder.Property(x => x.GlAccountCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.LaborLawReference)
            .HasMaxLength(200);


        builder.Property(x => x.DisplayOrder)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasIndex(x => x.ComponentType);

        builder.HasIndex(x => x.IsActive);

        builder.HasIndex(x => x.IsMandatory);

        // Optimized for mandatory component identification
        builder.HasIndex(x => new { x.IsMandatory, x.IsActive })
            .HasDatabaseName("IX_PayComponent_Mandatory_Active");

        // Calculation method queries
        builder.HasIndex(x => new { x.CalculationMethod, x.ComponentType, x.IsActive })
            .HasDatabaseName("IX_PayComponent_Calculation_Active");

        // GL account lookups
        builder.HasIndex(x => new { x.GlAccountCode, x.IsActive })
            .HasDatabaseName("IX_PayComponent_GlAccount_Active");

        // Component type filtering
        builder.HasIndex(x => new { x.ComponentType, x.IsActive })
            .HasDatabaseName("IX_PayComponent_Type_Active");

        // Relationships
        builder.HasMany(x => x.Rates)
            .WithOne(x => x.PayComponent)
            .HasForeignKey(x => x.PayComponentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

