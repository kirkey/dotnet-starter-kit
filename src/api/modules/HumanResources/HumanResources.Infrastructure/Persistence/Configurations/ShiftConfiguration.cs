using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.ShiftName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.WorkingHours)
            .HasPrecision(5, 2);

        builder.HasMany(s => s.Breaks)
            .WithOne(b => b.Shift)
            .HasForeignKey(b => b.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Assignments)
            .WithOne(a => a.Shift)
            .HasForeignKey(a => a.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.ShiftName)
            .HasDatabaseName("IX_Shift_ShiftName");

        builder.HasIndex(s => s.IsActive)
            .HasDatabaseName("IX_Shift_IsActive");
    }
}

public class ShiftBreakConfiguration : IEntityTypeConfiguration<ShiftBreak>
{
    public void Configure(EntityTypeBuilder<ShiftBreak> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BreakType)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(b => b.Shift)
            .WithMany(s => s.Breaks)
            .HasForeignKey(b => b.ShiftId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.ShiftId)
            .HasDatabaseName("IX_ShiftBreak_ShiftId");
    }
}

public class ShiftAssignmentConfiguration : IEntityTypeConfiguration<ShiftAssignment>
{
    public void Configure(EntityTypeBuilder<ShiftAssignment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Notes)
            .HasMaxLength(500);

        builder.HasOne(a => a.Employee)
            .WithMany(e => e.ShiftAssignments)
            .HasForeignKey(a => a.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Shift)
            .WithMany(s => s.Assignments)
            .HasForeignKey(a => a.ShiftId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => a.EmployeeId)
            .HasDatabaseName("IX_ShiftAssignment_EmployeeId");

        builder.HasIndex(a => a.ShiftId)
            .HasDatabaseName("IX_ShiftAssignment_ShiftId");

        builder.HasIndex(a => a.StartDate)
            .HasDatabaseName("IX_ShiftAssignment_StartDate");

        builder.HasIndex(a => new { a.EmployeeId, a.StartDate, a.EndDate })
            .HasDatabaseName("IX_ShiftAssignment_EmployeeId_Period");

        builder.HasIndex(a => a.IsActive)
            .HasDatabaseName("IX_ShiftAssignment_IsActive");
    }
}

