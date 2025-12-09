namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the FeeWaiver entity.
/// </summary>
internal sealed class FeeWaiverConfiguration : IEntityTypeConfiguration<FeeWaiver>
{
    public void Configure(EntityTypeBuilder<FeeWaiver> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reference)
            .HasMaxLength(FeeWaiver.ReferenceMaxLength);

        builder.Property(x => x.WaiverReason)
            .HasMaxLength(FeeWaiver.WaiverReasonMaxLength);

        builder.Property(x => x.WaiverType)
            .HasMaxLength(FeeWaiver.WaiverTypeMaxLength);

        builder.Property(x => x.ApprovedBy)
            .HasMaxLength(FeeWaiver.ApprovedByMaxLength);

        builder.Property(x => x.Status)
            .HasMaxLength(FeeWaiver.StatusMaxLength);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(FeeWaiver.RejectionReasonMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(FeeWaiver.NotesMaxLength);

        builder.Property(x => x.OriginalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.WaivedAmount)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.FeeCharge)
            .WithMany(x => x.Waivers)
            .HasForeignKey(x => x.FeeChargeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.FeeChargeId)
            .HasDatabaseName("IX_FeeWaivers_FeeChargeId");

        builder.HasIndex(x => x.RequestDate)
            .HasDatabaseName("IX_FeeWaivers_RequestDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_FeeWaivers_Status");
    }
}
