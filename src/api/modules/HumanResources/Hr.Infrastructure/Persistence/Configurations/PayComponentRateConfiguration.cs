using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PayComponentRateConfiguration : IEntityTypeConfiguration<PayComponentRate>
{
    public void Configure(EntityTypeBuilder<PayComponentRate> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.MinAmount)
            .IsRequired()
            .HasPrecision(15, 2);

        builder.Property(x => x.MaxAmount)
            .IsRequired()
            .HasPrecision(15, 2);

        builder.Property(x => x.EmployeeRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.EmployerRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.AdditionalEmployerRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.EmployeeAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.EmployerAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.TaxRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.BaseAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.ExcessRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.Year)
            .IsRequired();

        // Description handled by base class - use new modifier if needed
        // builder.Property(x => x.Description).HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => new { x.PayComponentId, x.Year, x.MinAmount, x.MaxAmount })
            .HasDatabaseName("IX_PayComponentRates_Component_Year_Range");

        builder.HasIndex(x => x.Year);

        builder.HasIndex(x => x.IsActive);

        // Relationships
        builder.HasOne(x => x.PayComponent)
            .WithMany(x => x.Rates)
            .HasForeignKey(x => x.PayComponentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
