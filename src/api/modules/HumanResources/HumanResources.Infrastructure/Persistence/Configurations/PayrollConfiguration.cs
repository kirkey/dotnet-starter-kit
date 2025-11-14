using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PayFrequency)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.TotalGrossPay)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalTaxes)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalDeductions)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalNetPay)
            .HasPrecision(12, 2);

        builder.Property(p => p.JournalEntryId)
            .HasMaxLength(50);

        builder.Property(p => p.Notes)
            .HasMaxLength(500);

        builder.HasMany(p => p.Lines)
            .WithOne(l => l.Payroll)
            .HasForeignKey(l => l.PayrollId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.StartDate)
            .HasDatabaseName("IX_Payroll_StartDate");

        builder.HasIndex(p => new { p.StartDate, p.EndDate })
            .HasDatabaseName("IX_Payroll_DateRange")
            .IsUnique();

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payroll_Status");

        builder.HasIndex(p => p.IsLocked)
            .HasDatabaseName("IX_Payroll_IsLocked");
    }
}

public class PayrollLineConfiguration : IEntityTypeConfiguration<PayrollLine>
{
    public void Configure(EntityTypeBuilder<PayrollLine> builder)
    {
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
    }
}

public class PayComponentConfiguration : IEntityTypeConfiguration<PayComponent>
{
    public void Configure(EntityTypeBuilder<PayComponent> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ComponentName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ComponentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.GlAccountCode)
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.HasIndex(c => c.ComponentName)
            .HasDatabaseName("IX_PayComponent_ComponentName");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_PayComponent_IsActive");
    }
}

public class TaxBracketConfiguration : IEntityTypeConfiguration<TaxBracket>
{
    public void Configure(EntityTypeBuilder<TaxBracket> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaxType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.MinIncome)
            .HasPrecision(10, 2);

        builder.Property(t => t.MaxIncome)
            .HasPrecision(10, 2);

        builder.Property(t => t.Rate)
            .HasPrecision(5, 4);

        builder.Property(t => t.FilingStatus)
            .HasMaxLength(50);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.HasIndex(t => new { t.TaxType, t.Year })
            .HasDatabaseName("IX_TaxBracket_TaxType_Year");
    }
}

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.HolidayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Description)
            .HasMaxLength(500);

        builder.HasIndex(h => h.HolidayDate)
            .HasDatabaseName("IX_Holiday_HolidayDate");

        builder.HasIndex(h => h.IsActive)
            .HasDatabaseName("IX_Holiday_IsActive");
    }
}

public class BenefitConfiguration : IEntityTypeConfiguration<Benefit>
{
    public void Configure(EntityTypeBuilder<Benefit> builder)
    {
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

public class BenefitEnrollmentConfiguration : IEntityTypeConfiguration<BenefitEnrollment>
{
    public void Configure(EntityTypeBuilder<BenefitEnrollment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CoverageLevel)
            .HasMaxLength(50);

        builder.Property(e => e.EmployeeContributionAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.EmployerContributionAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.CoveredDependentIds)
            .HasMaxLength(1000);

        builder.HasOne(e => e.Employee)
            .WithMany(emp => emp.BenefitEnrollments)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Benefit)
            .WithMany(b => b.Enrollments)
            .HasForeignKey(e => e.BenefitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_BenefitEnrollment_EmployeeId");

        builder.HasIndex(e => e.BenefitId)
            .HasDatabaseName("IX_BenefitEnrollment_BenefitId");

        builder.HasIndex(e => new { e.EmployeeId, e.BenefitId, e.EffectiveDate })
            .HasDatabaseName("IX_BenefitEnrollment_EmployeeId_BenefitId_EffectiveDate");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_BenefitEnrollment_IsActive");
    }
}

