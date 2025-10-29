using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Messaging.Persistence.Configurations;

/// <summary>
/// Entity configuration for the MessageAttachment entity.
/// </summary>
internal sealed class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.FileType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.ThumbnailUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.UploadedAt)
            .IsRequired();

        builder.HasOne(x => x.Message)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.MessageId);
        builder.HasIndex(x => x.FileType);
    }
}

