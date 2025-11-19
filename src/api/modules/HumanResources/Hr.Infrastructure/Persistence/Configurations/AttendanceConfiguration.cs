using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AttendanceDate)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.ClockInLocation)
            .HasMaxLength(500);

        builder.Property(a => a.ClockOutLocation)
            .HasMaxLength(500);

        builder.Property(a => a.Reason)
            .HasMaxLength(500);

        builder.Property(a => a.ManagerComment)
            .HasMaxLength(500);

        builder.Property(a => a.HoursWorked)
            .HasPrecision(5, 2);

        builder.HasOne(a => a.Employee)
            .WithMany(e => e.AttendanceRecords)
            .HasForeignKey(a => a.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.EmployeeId)
            .HasDatabaseName("IX_Attendance_EmployeeId");

        builder.HasIndex(a => a.AttendanceDate)
            .HasDatabaseName("IX_Attendance_AttendanceDate");

        builder.HasIndex(a => new { a.EmployeeId, a.AttendanceDate })
            .HasDatabaseName("IX_Attendance_EmployeeId_AttendanceDate")
            .IsUnique();

        builder.HasIndex(a => a.Status)
            .HasDatabaseName("IX_Attendance_Status");

        builder.HasIndex(a => a.IsApproved)
            .HasDatabaseName("IX_Attendance_IsApproved");

        builder.HasIndex(a => a.IsActive)
            .HasDatabaseName("IX_Attendance_IsActive");
    }
}

