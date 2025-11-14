using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class EmployeePayComponentConfiguration : IEntityTypeConfiguration<EmployeePayComponent>
{
    public void Configure(EntityTypeBuilder<EmployeePayComponent> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignmentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CustomRate)
            .HasPrecision(18, 6);

        builder.Property(x => x.FixedAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.CustomFormula)
            .HasMaxLength(500);

        builder.Property(x => x.EffectiveStartDate)
            .IsRequired();

        builder.Property(x => x.TotalAmount)
            .HasPrecision(15, 2);

        builder.Property(x => x.RemainingBalance)
            .HasPrecision(15, 2);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(x => x.EmployeeId);

        builder.HasIndex(x => new { x.EmployeeId, x.PayComponentId, x.IsActive })
            .HasDatabaseName("IX_EmployeePayComponents_Employee_Component_Active");

        builder.HasIndex(x => x.ReferenceNumber);

        builder.HasIndex(x => new { x.EffectiveStartDate, x.EffectiveEndDate })
            .HasDatabaseName("IX_EmployeePayComponents_DateRange");

        // Relationships
        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PayComponent)
            .WithMany()
            .HasForeignKey(x => x.PayComponentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
