using System.Diagnostics.Metrics;

namespace FSH.Starter.WebApi.Messaging.Domain;

/// <summary>
/// Provides metrics for monitoring messaging module operations.
/// </summary>
public static class MessagingMetrics
{
    private static readonly Meter Meter = new("FSH.Starter.Messaging", "1.0.0");

    public static readonly Counter<int> ConversationCreated = Meter.CreateCounter<int>(
        "messaging.conversations.created",
        description: "Number of conversations created");

    public static readonly Counter<int> ConversationUpdated = Meter.CreateCounter<int>(
        "messaging.conversations.updated",
        description: "Number of conversations updated");

    public static readonly Counter<int> MessageSent = Meter.CreateCounter<int>(
        "messaging.messages.sent",
        description: "Number of messages sent");

    public static readonly Counter<int> MessageUpdated = Meter.CreateCounter<int>(
        "messaging.messages.updated",
        description: "Number of messages updated");

    public static readonly Counter<int> MessageDeleted = Meter.CreateCounter<int>(
        "messaging.messages.deleted",
        description: "Number of messages deleted");

    public static readonly Counter<int> MemberAdded = Meter.CreateCounter<int>(
        "messaging.members.added",
        description: "Number of members added to conversations");

    public static readonly Counter<int> MemberRemoved = Meter.CreateCounter<int>(
        "messaging.members.removed",
        description: "Number of members removed from conversations");

    public static readonly Counter<int> MemberRoleUpdated = Meter.CreateCounter<int>(
        "messaging.members.role_updated",
        description: "Number of member role updates");

    public static readonly Counter<int> AttachmentAdded = Meter.CreateCounter<int>(
        "messaging.attachments.added",
        description: "Number of attachments added to messages");
}

