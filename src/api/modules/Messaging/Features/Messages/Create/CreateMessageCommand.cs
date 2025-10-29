using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.Create;

/// <summary>
/// Command for creating a new message.
/// </summary>
public record CreateMessageCommand(
    DefaultIdType ConversationId,
    [property: DefaultValue("Hello, this is a message!")] string Content,
    [property: DefaultValue("text")] string MessageType = "text",
    DefaultIdType? ReplyToMessageId = null,
    List<FileUploadCommand>? Attachments = null) : IRequest<CreateMessageResponse>;
