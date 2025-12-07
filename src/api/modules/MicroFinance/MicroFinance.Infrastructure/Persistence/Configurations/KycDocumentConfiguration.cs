namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the KycDocument entity.
/// </summary>
internal sealed class KycDocumentConfiguration : IEntityTypeConfiguration<KycDocument>
{
    public void Configure(EntityTypeBuilder<KycDocument> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.DocumentType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.DocumentNumber)
            .HasMaxLength(128);

        builder.Property(x => x.FilePath)
            .HasMaxLength(1024);

        builder.Property(x => x.FileName)
            .HasMaxLength(512);

        builder.Property(x => x.MimeType)
            .HasMaxLength(128);

        builder.Property(x => x.RejectionReason)
            .HasMaxLength(1024);

        builder.Property(x => x.Notes)
            .HasMaxLength(4096);

        // Relationships
        builder.HasOne(x => x.Member)
            .WithMany()
            .HasForeignKey(x => x.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.VerifiedById)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.MemberId)
            .HasDatabaseName("IX_KycDocuments_MemberId");

        builder.HasIndex(x => x.DocumentType)
            .HasDatabaseName("IX_KycDocuments_DocumentType");

        builder.HasIndex(x => new { x.MemberId, x.DocumentType })
            .HasDatabaseName("IX_KycDocuments_MemberDocument");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_KycDocuments_Status");
    }
}
