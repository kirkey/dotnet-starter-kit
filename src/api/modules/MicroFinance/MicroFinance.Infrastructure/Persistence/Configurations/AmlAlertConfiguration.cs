namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the AmlAlert entity.
/// </summary>
internal sealed class AmlAlertConfiguration : IEntityTypeConfiguration<AmlAlert>
{
    public void Configure(EntityTypeBuilder<AmlAlert> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.TransactionAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.AlertCode)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.AlertType)
            .HasMaxLength(64);

        builder.Property(x => x.Severity)
            .HasMaxLength(32);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.TriggerRule)
            .HasMaxLength(256);

        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        builder.Property(x => x.ResolutionNotes)
            .HasMaxLength(4096);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.ResolvedById)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.AlertCode)
            .IsUnique()
            .HasDatabaseName("IX_AmlAlerts_AlertCode");

        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_AmlAlerts_MemberId");

        builder.HasIndex(x => x.AlertType)
            .HasDatabaseName("IX_AmlAlerts_AlertType");

        builder.HasIndex(x => x.Severity)
            .HasDatabaseName("IX_AmlAlerts_Severity");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_AmlAlerts_Status");

        builder.HasIndex(x => x.AssignedToId)
            .HasDatabaseName("IX_AmlAlerts_AssignedToId");
    }
}
