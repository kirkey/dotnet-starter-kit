using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}

