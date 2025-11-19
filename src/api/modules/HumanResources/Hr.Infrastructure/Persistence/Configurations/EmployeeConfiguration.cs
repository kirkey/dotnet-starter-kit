using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .HasMaxLength(256);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TerminationReason)
            .HasMaxLength(500);

        builder.HasOne(e => e.OrganizationalUnit)
            .WithMany()
            .HasForeignKey(e => e.OrganizationalUnitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.DesignationAssignments)
            .WithOne(d => d.Employee)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Contacts)
            .WithOne(c => c.Employee)
            .HasForeignKey(c => c.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Dependents)
            .WithOne(d => d.Employee)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Documents)
            .WithOne(d => d.Employee)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AttendanceRecords)
            .WithOne(a => a.Employee)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Timesheets)
            .WithOne(t => t.Employee)
            .HasForeignKey(t => t.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ShiftAssignments)
            .WithOne(s => s.Employee)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LeaveBalances)
            .WithOne(l => l.Employee)
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LeaveRequests)
            .WithOne(l => l.Employee)
            .HasForeignKey(l => l.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PayrollLines)
            .WithOne(p => p.Employee)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.BenefitEnrollments)
            .WithOne(b => b.Employee)
            .HasForeignKey(b => b.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.EmployeeNumber)
            .HasDatabaseName("IX_Employee_EmployeeNumber")
            .IsUnique();

        builder.HasIndex(e => e.OrganizationalUnitId)
            .HasDatabaseName("IX_Employee_OrganizationalUnitId");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("IX_Employee_Status");

        builder.HasIndex(e => e.Email)
            .HasDatabaseName("IX_Employee_Email");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Employee_IsActive");

        builder.HasIndex(e => new { e.FirstName, e.LastName })
            .HasDatabaseName("IX_Employee_FirstName_LastName");

        // Optimized indexes for multi-tenant + status filtering
        builder.HasIndex(e => new { e.Status, e.IsActive })
            .HasDatabaseName("IX_Employee_Status_Active");

        // Organizational unit + department filtering
        builder.HasIndex(e => new { e.OrganizationalUnitId, e.Status, e.IsActive })
            .HasDatabaseName("IX_Employee_OrgUnit_Status_Active");

        // Email lookup with active filter (covering index)
        builder.HasIndex(e => new { e.Email, e.IsActive })
            .HasDatabaseName("IX_Employee_Email_Active");

        // Last name + first name composite for name searches
        builder.HasIndex(e => new { e.LastName, e.FirstName, e.IsActive })
            .HasDatabaseName("IX_Employee_LastName_FirstName_Active");
    }
}

