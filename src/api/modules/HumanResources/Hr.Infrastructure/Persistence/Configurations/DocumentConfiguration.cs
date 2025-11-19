using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.TemplateName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.DocumentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.TemplateContent)
            .IsRequired();

        builder.Property(d => d.TemplateVariables)
            .HasMaxLength(1000);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        builder.HasIndex(d => d.TemplateName)
            .HasDatabaseName("IX_DocumentTemplate_TemplateName");

        builder.HasIndex(d => d.DocumentType)
            .HasDatabaseName("IX_DocumentTemplate_DocumentType");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_DocumentTemplate_IsActive");

        // Optimized for document type browsing
        builder.HasIndex(d => new { d.DocumentType, d.IsActive })
            .HasDatabaseName("IX_DocumentTemplate_Type_Active");

        // Template availability
        builder.HasIndex(d => new { d.IsActive, d.TemplateName })
            .HasDatabaseName("IX_DocumentTemplate_Active_Name");
    }
}

public class GeneratedDocumentConfiguration : IEntityTypeConfiguration<GeneratedDocument>
{
    public void Configure(EntityTypeBuilder<GeneratedDocument> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.GeneratedContent)
            .IsRequired();

        builder.Property(d => d.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.SignedBy)
            .HasMaxLength(200);

        builder.Property(d => d.SignatureMetadata)
            .HasMaxLength(1000);

        builder.Property(d => d.FilePath)
            .HasMaxLength(1000);

        builder.Property(d => d.Notes)
            .HasMaxLength(1000);

        builder.HasOne(d => d.DocumentTemplate)
            .WithMany()
            .HasForeignKey(d => d.DocumentTemplateId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => d.EntityId)
            .HasDatabaseName("IX_GeneratedDocument_EntityId");

        builder.HasIndex(d => new { d.EntityId, d.EntityType })
            .HasDatabaseName("IX_GeneratedDocument_EntityId_EntityType");

        builder.HasIndex(d => d.Status)
            .HasDatabaseName("IX_GeneratedDocument_Status");

        builder.HasIndex(d => d.GeneratedDate)
            .HasDatabaseName("IX_GeneratedDocument_GeneratedDate");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_GeneratedDocument_IsActive");

        // Optimized for entity-specific document queries
        builder.HasIndex(d => new { d.EntityId, d.EntityType, d.GeneratedDate })
            .HasDatabaseName("IX_GeneratedDocument_Entity_Date");

        // Approval tracking
        builder.HasIndex(d => new { d.Status, d.GeneratedDate })
            .HasDatabaseName("IX_GeneratedDocument_Status_Date");

        // Signature tracking
        builder.HasIndex(d => new { d.SignedBy, d.GeneratedDate })
            .HasDatabaseName("IX_GeneratedDocument_SignedBy_Date");

        // Status + IsActive filtering
        builder.HasIndex(d => new { d.Status, d.IsActive })
            .HasDatabaseName("IX_GeneratedDocument_Status_Active");
    }
}

