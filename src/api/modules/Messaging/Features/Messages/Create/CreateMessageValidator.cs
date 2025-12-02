namespace FSH.Starter.WebApi.Messaging.Features.Messages.Create;

/// <summary>
/// Validator for CreateMessageCommand with strict validation rules.
/// </summary>
public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator(MessagingDbContext context)
    {
        RuleFor(m => m.ConversationId)
            .NotEmpty()
            .WithMessage("conversation id is required");

        RuleFor(m => m.Content)
            .NotEmpty()
            .WithMessage("message content is required")
            .MaximumLength(8192)
            .WithMessage("message content must not exceed 8192 characters");

        RuleFor(m => m.MessageType)
            .NotEmpty()
            .WithMessage("message type is required")
            .Must(type => new[] { MessageTypes.Text, MessageTypes.File, MessageTypes.Image, 
                MessageTypes.Video, MessageTypes.Audio, MessageTypes.Document }.Contains(type))
            .WithMessage("invalid message type");

        When(m => m.Attachments != null && m.Attachments.Any(), () =>
        {
            RuleFor(m => m.Attachments)
                .Must(attachments => attachments!.Count <= 10)
                .WithMessage("maximum 10 attachments allowed per message");
        });
    }
}

