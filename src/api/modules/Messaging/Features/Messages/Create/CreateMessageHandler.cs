using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.Create;

/// <summary>
/// Handler for creating a new message.
/// Implements CQRS pattern and supports file attachments.
/// </summary>
public sealed class CreateMessageHandler(
    ILogger<CreateMessageHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Message> messageRepository,
    [FromKeyedServices("messaging")] IRepository<Conversation> conversationRepository,
    IStorageService storageService,
    ICurrentUser currentUser)
    : IRequestHandler<CreateMessageCommand, CreateMessageResponse>
{
    public async Task<CreateMessageResponse> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        // Verify conversation exists and user is a member
        var conversation = await conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        var isMember = conversation.Members.Any(m => m.UserId == currentUserId && m.IsActive);
        if (!isMember)
        {
            throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
        }

        // Create the message
        Message message;
        if (request.MessageType == MessageTypes.Text)
        {
            message = Message.CreateText(request.ConversationId, currentUserId, request.Content, request.ReplyToMessageId);
        }
        else
        {
            message = Message.CreateWithAttachments(request.ConversationId, currentUserId, request.Content, request.MessageType);
        }

        // Handle file attachments
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            foreach (var attachment in request.Attachments)
            {
                if (attachment.IsValid())
                {
                    // Determine file type for storage
                    var fileType = DetermineFileType(request.MessageType, attachment.Extension);
                    
                    // Upload file
                    var fileUri = await storageService.UploadAsync<FileUploadCommand>(attachment, fileType, cancellationToken).ConfigureAwait(false);

                    // Add attachment to message
                    message.AddAttachment(
                        fileUri.ToString(),
                        attachment.Name,
                        attachment.Extension,
                        attachment.Size ?? attachment.GetEstimatedSizeFromData(),
                        null); // Can add thumbnail logic later
                }
            }
        }

        await messageRepository.AddAsync(message, cancellationToken).ConfigureAwait(false);

        // Update conversation's last message timestamp
        conversation.UpdateLastMessageAt(message.SentAt);
        await conversationRepository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);

        await messageRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("message {MessageId} created in conversation {ConversationId} by user {UserId}", 
            message.Id, request.ConversationId, currentUserId);

        return new CreateMessageResponse(message.Id, message.SentAt);
    }

    private static FileType DetermineFileType(string messageType, string extension)
    {
        return messageType switch
        {
            MessageTypes.Image => FileType.Image,
            MessageTypes.Video => FileType.Document, // Video files treated as documents
            MessageTypes.Audio => FileType.Document, // Audio files treated as documents
            MessageTypes.Document => FileType.Document,
            MessageTypes.File => FileType.ZipFile, // Generic files as zip
            _ => FileType.Document
        };
    }
}

