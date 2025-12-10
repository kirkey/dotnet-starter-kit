namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Staff entity.
/// </summary>
internal sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeNumber)
            .HasMaxLength(Staff.MaxLengths.EmployeeNumber);

        builder.Property(x => x.FirstName)
            .HasMaxLength(Staff.MaxLengths.FirstName);

        builder.Property(x => x.LastName)
            .HasMaxLength(Staff.MaxLengths.LastName);

        builder.Property(x => x.MiddleName)
            .HasMaxLength(Staff.MaxLengths.MiddleName);

        builder.Property(x => x.Email)
            .HasMaxLength(Staff.MaxLengths.Email);

        builder.Property(x => x.Phone)
            .HasMaxLength(Staff.MaxLengths.Phone);

        builder.Property(x => x.AlternatePhone)
            .HasMaxLength(Staff.MaxLengths.AlternatePhone);

        builder.Property(x => x.NationalId)
            .HasMaxLength(Staff.MaxLengths.NationalId);

        builder.Property(x => x.Address)
            .HasMaxLength(Staff.MaxLengths.Address);

        builder.Property(x => x.Department)
            .HasMaxLength(Staff.MaxLengths.Department);

        builder.Property(x => x.JobTitle)
            .HasMaxLength(Staff.MaxLengths.JobTitle);

        builder.Property(x => x.Designation)
            .HasMaxLength(Staff.MaxLengths.Designation);

        builder.Property(x => x.ReportingTo)
            .HasMaxLength(Staff.MaxLengths.ReportingTo);

        builder.Property(x => x.BasicSalary)
            .HasPrecision(18, 2);

        builder.Property(x => x.BankAccountNumber)
            .HasMaxLength(Staff.MaxLengths.BankAccountNumber);

        builder.Property(x => x.BankName)
            .HasMaxLength(Staff.MaxLengths.BankName);

        builder.Property(x => x.EmergencyContactName)
            .HasMaxLength(Staff.MaxLengths.EmergencyContactName);

        builder.Property(x => x.EmergencyContactPhone)
            .HasMaxLength(Staff.MaxLengths.EmergencyContactPhone);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.LoanApprovalLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.Notes)
            .HasMaxLength(Staff.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReportingManager)
            .WithMany(x => x.DirectReports)
            .HasForeignKey(x => x.ReportingManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.ReportingManagerId);
        builder.HasIndex(x => x.Status);
    }
}
