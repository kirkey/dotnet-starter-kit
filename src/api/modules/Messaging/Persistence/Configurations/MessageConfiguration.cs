using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Messaging.Persistence.Configurations;

/// <summary>
/// Entity configuration for the Message entity.
/// </summary>
internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(x => x.MessageType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.SentAt)
            .IsRequired();

        builder.Property(x => x.IsEdited)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.EditedAt);

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DeletedAt);

        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ReplyToMessage)
            .WithMany()
            .HasForeignKey(x => x.ReplyToMessageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Attachments)
            .WithOne(x => x.Message)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ConversationId);
        builder.HasIndex(x => x.SenderId);
        builder.HasIndex(x => x.SentAt);
        builder.HasIndex(x => x.IsDeleted);
        builder.HasIndex(x => new { x.ConversationId, x.SentAt });
    }
}

