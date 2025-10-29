using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Starter.WebApi.Messaging.Persistence.Configurations;

/// <summary>
/// Entity configuration for the ConversationMember entity.
/// </summary>
internal sealed class ConversationMemberConfiguration : IEntityTypeConfiguration<ConversationMember>
{
    public void Configure(EntityTypeBuilder<ConversationMember> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.JoinedAt)
            .IsRequired();

        builder.Property(x => x.LeftAt);

        builder.Property(x => x.LastReadAt);

        builder.Property(x => x.IsMuted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ConversationId);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => new { x.ConversationId, x.UserId, x.IsActive });
    }
}

