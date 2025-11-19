using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PayrollLineConfiguration : IEntityTypeConfiguration<PayrollLine>
{
    public void Configure(EntityTypeBuilder<PayrollLine> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(l => l.Id);

        builder.Property(l => l.RegularHours)
            .HasPrecision(5, 2);

        builder.Property(l => l.OvertimeHours)
            .HasPrecision(5, 2);

        builder.Property(l => l.RegularPay)
            .HasPrecision(10, 2);

        builder.Property(l => l.OvertimePay)
            .HasPrecision(10, 2);

        builder.Property(l => l.BonusPay)
            .HasPrecision(10, 2);

        builder.Property(l => l.OtherEarnings)
            .HasPrecision(10, 2);

        builder.Property(l => l.GrossPay)
            .HasPrecision(10, 2);

        builder.Property(l => l.IncomeTax)
            .HasPrecision(10, 2);

        builder.Property(l => l.SocialSecurityTax)
            .HasPrecision(10, 2);

        builder.Property(l => l.MedicareTax)
            .HasPrecision(10, 2);

        builder.Property(l => l.OtherTaxes)
            .HasPrecision(10, 2);

        builder.Property(l => l.TotalTaxes)
            .HasPrecision(10, 2);

        builder.Property(l => l.HealthInsurance)
            .HasPrecision(10, 2);

        builder.Property(l => l.RetirementContribution)
            .HasPrecision(10, 2);

        builder.Property(l => l.OtherDeductions)
            .HasPrecision(10, 2);

        builder.Property(l => l.TotalDeductions)
            .HasPrecision(10, 2);

        builder.Property(l => l.NetPay)
            .HasPrecision(10, 2);

        builder.Property(l => l.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(l => l.BankAccountLast4)
            .HasMaxLength(4);

        builder.Property(l => l.CheckNumber)
            .HasMaxLength(20);

        builder.HasOne(l => l.Payroll)
            .WithMany(p => p.Lines)
            .HasForeignKey(l => l.PayrollId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Employee)
            .WithMany(e => e.PayrollLines)
            .HasForeignKey(l => l.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.PayrollId)
            .HasDatabaseName("IX_PayrollLine_PayrollId");

        builder.HasIndex(l => l.EmployeeId)
            .HasDatabaseName("IX_PayrollLine_EmployeeId");

        builder.HasIndex(l => new { l.PayrollId, l.EmployeeId })
            .HasDatabaseName("IX_PayrollLine_PayrollId_EmployeeId")
            .IsUnique();

        // Optimized for payroll completion tracking
        builder.HasIndex(l => new { l.PayrollId, l.EmployeeId })
            .HasDatabaseName("IX_PayrollLine_Completion");

        // Payment method tracking
        builder.HasIndex(l => new { l.PaymentMethod })
            .HasDatabaseName("IX_PayrollLine_PaymentMethod");
    }
}

