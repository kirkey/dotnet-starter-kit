using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.LeaveName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Description)
            .HasMaxLength(500);

        builder.Property(l => l.AnnualAllowance)
            .HasPrecision(5, 2);

        builder.Property(l => l.MaxCarryoverDays)
            .HasPrecision(5, 2);

        builder.Property(l => l.AccrualFrequency)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(l => l.LeaveName)
            .HasDatabaseName("IX_LeaveType_LeaveName");

        builder.HasIndex(l => l.IsActive)
            .HasDatabaseName("IX_LeaveType_IsActive");
    }
}

public class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
{
    public void Configure(EntityTypeBuilder<LeaveBalance> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.OpeningBalance)
            .HasPrecision(5, 2);

        builder.Property(l => l.AccruedDays)
            .HasPrecision(5, 2);

        builder.Property(l => l.CarriedOverDays)
            .HasPrecision(5, 2);

        builder.Property(l => l.TakenDays)
            .HasPrecision(5, 2);

        builder.Property(l => l.PendingDays)
            .HasPrecision(5, 2);

        builder.HasOne(l => l.Employee)
            .WithMany(e => e.LeaveBalances)
            .HasForeignKey(l => l.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.LeaveType)
            .WithMany()
            .HasForeignKey(l => l.LeaveTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.EmployeeId)
            .HasDatabaseName("IX_LeaveBalance_EmployeeId");

        builder.HasIndex(l => l.LeaveTypeId)
            .HasDatabaseName("IX_LeaveBalance_LeaveTypeId");

        builder.HasIndex(l => new { l.EmployeeId, l.LeaveTypeId, l.Year })
            .HasDatabaseName("IX_LeaveBalance_EmployeeId_LeaveTypeId_Year")
            .IsUnique();
    }
}

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Reason)
            .HasMaxLength(500);

        builder.Property(l => l.ApproverComment)
            .HasMaxLength(500);

        builder.Property(l => l.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.NumberOfDays)
            .HasPrecision(5, 2);

        builder.Property(l => l.AttachmentPath)
            .HasMaxLength(1000);

        builder.HasOne(l => l.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(l => l.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.LeaveType)
            .WithMany()
            .HasForeignKey(l => l.LeaveTypeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.EmployeeId)
            .HasDatabaseName("IX_LeaveRequest_EmployeeId");

        builder.HasIndex(l => l.LeaveTypeId)
            .HasDatabaseName("IX_LeaveRequest_LeaveTypeId");

        builder.HasIndex(l => l.Status)
            .HasDatabaseName("IX_LeaveRequest_Status");

        builder.HasIndex(l => new { l.EmployeeId, l.StartDate, l.EndDate })
            .HasDatabaseName("IX_LeaveRequest_EmployeeId_DateRange");

        builder.HasIndex(l => l.IsActive)
            .HasDatabaseName("IX_LeaveRequest_IsActive");
    }
}

