namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the AgentBanking entity.
/// </summary>
internal sealed class AgentBankingConfiguration : IEntityTypeConfiguration<AgentBanking>
{
    public void Configure(EntityTypeBuilder<AgentBanking> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.FloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MinFloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.MaxFloatBalance)
            .HasPrecision(18, 2);

        builder.Property(x => x.CommissionRate)
            .HasPrecision(18, 2);

        builder.Property(x => x.TotalCommissionEarned)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyTransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyTransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(x => x.DailyVolumeProcessed)
            .HasPrecision(18, 2);

        builder.Property(x => x.MonthlyVolumeProcessed)
            .HasPrecision(18, 2);

        builder.Property(x => x.AgentCode)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.BusinessName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.ContactName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(x => x.Email)
            .HasMaxLength(256);

        builder.Property(x => x.Address)
            .HasMaxLength(512);

        builder.Property(x => x.GpsCoordinates)
            .HasMaxLength(128);

        builder.Property(x => x.Tier)
            .HasMaxLength(32);

        builder.Property(x => x.DeviceId)
            .HasMaxLength(256);

        builder.Property(x => x.OperatingHours)
            .HasMaxLength(256);

        // Relationships
        builder.HasOne<Branch>()
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.LinkedStaffId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.AgentCode)
            .IsUnique()
            .HasDatabaseName("IX_AgentBankings_AgentCode");

        builder.HasIndex(x => x.BranchId)
            .HasDatabaseName("IX_AgentBankings_BranchId");

        builder.HasIndex(x => x.LinkedStaffId)
            .HasDatabaseName("IX_AgentBankings_LinkedStaffId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_AgentBankings_Status");
    }
}
