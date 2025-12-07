namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for the Document entity.
/// </summary>
internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);


        builder.Property(x => x.Status)
            .HasMaxLength(128);

        builder.Property(x => x.DocumentType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.EntityType)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(x => x.MimeType)
            .HasMaxLength(128);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(512);

        builder.Property(x => x.IssuingAuthority)
            .HasMaxLength(256);

        builder.Property(x => x.DocumentNumber)
            .HasMaxLength(128);

        builder.Property(x => x.Category)
            .HasMaxLength(64);

        builder.Property(x => x.Tags)
            .HasMaxLength(512);

        // Relationships
        builder.HasOne<Staff>()
            .WithMany()
            .HasForeignKey(x => x.VerifiedById)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => x.EntityId)
            .HasDatabaseName("IX_Documents_EntityId");

        builder.HasIndex(x => new { x.EntityType, x.EntityId })
            .HasDatabaseName("IX_Documents_Entity");

        builder.HasIndex(x => x.VerifiedById)
            .HasDatabaseName("IX_Documents_VerifiedById");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Documents_Status");

        builder.HasIndex(x => x.DocumentType)
            .HasDatabaseName("IX_Documents_DocumentType");
    }
}
