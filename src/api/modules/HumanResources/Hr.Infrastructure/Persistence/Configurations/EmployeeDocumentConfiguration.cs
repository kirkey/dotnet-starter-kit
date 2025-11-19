using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

public class EmployeeDocumentConfiguration : IEntityTypeConfiguration<EmployeeDocument>
{
    public void Configure(EntityTypeBuilder<EmployeeDocument> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.FileName)
            .HasMaxLength(256);

        builder.Property(d => d.FilePath)
            .HasMaxLength(1000);

        builder.Property(d => d.IssueNumber)
            .HasMaxLength(100);

        builder.Property(d => d.Notes)
            .HasMaxLength(1000);

        builder.Property(d => d.Version)
            .HasDefaultValue(1);

        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Documents)
            .HasForeignKey(d => d.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.EmployeeId)
            .HasDatabaseName("IX_EmployeeDocuments_EmployeeId");

        builder.HasIndex(d => d.DocumentType)
            .HasDatabaseName("IX_EmployeeDocuments_DocumentType");

        builder.HasIndex(d => new { d.EmployeeId, d.DocumentType })
            .HasDatabaseName("IX_EmployeeDocuments_EmployeeId_DocumentType");

        builder.HasIndex(d => d.ExpiryDate)
            .HasDatabaseName("IX_EmployeeDocuments_ExpiryDate");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_EmployeeDocuments_IsActive");
    }
}

