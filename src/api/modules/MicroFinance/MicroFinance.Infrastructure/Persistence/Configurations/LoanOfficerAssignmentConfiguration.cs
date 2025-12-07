namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the LoanOfficerAssignment entity.
/// </summary>
internal sealed class LoanOfficerAssignmentConfiguration : IEntityTypeConfiguration<LoanOfficerAssignment>
{
    public void Configure(EntityTypeBuilder<LoanOfficerAssignment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AssignmentType)
            .HasMaxLength(LoanOfficerAssignment.MaxLengths.AssignmentType);

        builder.Property(x => x.Reason)
            .HasMaxLength(LoanOfficerAssignment.MaxLengths.Reason);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.Notes)
            .HasMaxLength(LoanOfficerAssignment.MaxLengths.Notes);

        // Relationships
        builder.HasOne(x => x.Staff)
            .WithMany(x => x.LoanOfficerAssignments)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MemberGroup)
            .WithMany()
            .HasForeignKey(x => x.MemberGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PreviousStaff)
            .WithMany()
            .HasForeignKey(x => x.PreviousStaffId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.StaffId);
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.MemberGroupId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.BranchId);
        builder.HasIndex(x => x.Status);
    }
}
