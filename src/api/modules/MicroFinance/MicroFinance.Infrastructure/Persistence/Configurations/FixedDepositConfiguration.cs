namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FixedDeposit entity.
/// </summary>
internal sealed class FixedDepositConfiguration : IEntityTypeConfiguration<FixedDeposit>
{
    public void Configure(EntityTypeBuilder<FixedDeposit> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(32);

        builder.Property(x => x.PrincipalAmount).HasPrecision(18, 2);
        builder.Property(x => x.InterestRate).HasPrecision(8, 4);
        builder.Property(x => x.InterestEarned).HasPrecision(18, 2);

        // Relationships
        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_FixedDeposits_MemberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FixedDeposits_Status");

        builder.HasIndex(x => x.MaturityDate)
            .HasDatabaseName("IX_FixedDeposits_MaturityDate");
    }
}

