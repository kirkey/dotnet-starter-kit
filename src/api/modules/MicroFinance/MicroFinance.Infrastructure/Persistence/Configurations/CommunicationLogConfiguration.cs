namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the CommunicationLog entity.
/// </summary>
internal sealed class CommunicationLogConfiguration : IEntityTypeConfiguration<CommunicationLog>
{
    public void Configure(EntityTypeBuilder<CommunicationLog> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Channel)
            .HasMaxLength(CommunicationLog.MaxLengths.Channel);

        builder.Property(x => x.Recipient)
            .HasMaxLength(CommunicationLog.MaxLengths.Recipient);

        builder.Property(x => x.Subject)
            .HasMaxLength(CommunicationLog.MaxLengths.Subject);

        builder.Property(x => x.Body)
            .HasMaxLength(CommunicationLog.MaxLengths.Body);

        builder.Property(x => x.DeliveryStatus)
            .HasMaxLength(CommunicationLog.MaxLengths.DeliveryStatus);

        builder.Property(x => x.ErrorMessage)
            .HasMaxLength(CommunicationLog.MaxLengths.ErrorMessage);

        builder.Property(x => x.ExternalId)
            .HasMaxLength(CommunicationLog.MaxLengths.ExternalId);

        builder.Property(x => x.Cost)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Loan)
            .WithMany()
            .HasForeignKey(x => x.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.TemplateId);
        builder.HasIndex(x => x.MemberId);
        builder.HasIndex(x => x.LoanId);
        builder.HasIndex(x => x.SentByUserId);
    }
}