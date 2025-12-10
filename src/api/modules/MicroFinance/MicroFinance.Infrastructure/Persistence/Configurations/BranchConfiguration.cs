namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Branch entity.
/// </summary>
internal sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(Branch.MaxLengths.Code);

        builder.Property(x => x.Address)
            .HasMaxLength(Branch.MaxLengths.Address);

        builder.Property(x => x.Phone)
            .HasMaxLength(Branch.MaxLengths.Phone);

        builder.Property(x => x.Email)
            .HasMaxLength(Branch.MaxLengths.Email);

        builder.Property(x => x.BranchType)
            .HasMaxLength(Branch.MaxLengths.BranchType);

        builder.Property(x => x.ManagerName)
            .HasMaxLength(Branch.MaxLengths.ManagerName);

        builder.Property(x => x.ManagerPhone)
            .HasMaxLength(Branch.MaxLengths.ManagerPhone);

        builder.Property(x => x.ManagerEmail)
            .HasMaxLength(Branch.MaxLengths.ManagerEmail);

        builder.Property(x => x.Latitude)
            .HasPrecision(18, 2);

        builder.Property(x => x.Longitude)
            .HasPrecision(18, 2);

        builder.Property(x => x.Timezone)
            .HasMaxLength(Branch.MaxLengths.Timezone);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.CashHoldingLimit)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.ParentBranch)
            .WithMany(x => x.ChildBranches)
            .HasForeignKey(x => x.ParentBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ParentBranchId);
        builder.HasIndex(x => x.Status);
    }
}
