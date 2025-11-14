using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class BenefitConfiguration : IEntityTypeConfiguration<Benefit>
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BenefitName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.BenefitType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.EmployeeContribution)
            .HasPrecision(10, 2);

        builder.Property(b => b.EmployerContribution)
            .HasPrecision(10, 2);

        builder.Property(b => b.AnnualLimit)
            .HasPrecision(10, 2);

        builder.Property(b => b.Description)
            .HasMaxLength(500);

        builder.HasMany(b => b.Enrollments)
            .WithOne(e => e.Benefit)
            .HasForeignKey(e => e.BenefitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.BenefitName)
            .HasDatabaseName("IX_Benefit_BenefitName");

        builder.HasIndex(b => b.IsActive)
            .HasDatabaseName("IX_Benefit_IsActive");
    }
}

