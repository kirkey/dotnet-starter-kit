using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.Messaging.Persistence;

/// <summary>
/// Database context for the Messaging module.
/// </summary>
public sealed class MessagingDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<MessagingDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    /// <summary>
    /// Gets or sets the Conversations DbSet.
    /// </summary>
    public DbSet<Conversation> Conversations { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ConversationMembers DbSet.
    /// </summary>
    public DbSet<ConversationMember> ConversationMembers { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Messages DbSet.
    /// </summary>
    public DbSet<Message> Messages { get; set; } = null!;

    /// <summary>
    /// Gets or sets the MessageAttachments DbSet.
    /// </summary>
    public DbSet<MessageAttachment> MessageAttachments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessagingDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Messaging);
    }
}
