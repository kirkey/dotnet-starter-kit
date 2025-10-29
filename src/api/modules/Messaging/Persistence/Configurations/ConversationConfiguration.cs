using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Messaging.Persistence.Configurations;

/// <summary>
/// Entity configuration for the Conversation entity.
/// </summary>
internal sealed class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200);

        builder.Property(x => x.ConversationType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.LastMessageAt);

        builder.HasMany(x => x.Members)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Conversation)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ConversationType);
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.LastMessageAt);
        builder.HasIndex(x => x.CreatedByUserId);
    }
}
