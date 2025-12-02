namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Create;

/// <summary>
/// Validator for CreateConversationCommand with strict validation rules.
/// </summary>
public class CreateConversationValidator : AbstractValidator<CreateConversationCommand>
{
    public CreateConversationValidator(MessagingDbContext context)
    {
        RuleFor(c => c.ConversationType)
            .NotEmpty()
            .WithMessage("conversation type is required")
            .Must(type => type == ConversationTypes.Direct || type == ConversationTypes.Group)
            .WithMessage("conversation type must be either 'direct' or 'group'");

        When(c => c.ConversationType == ConversationTypes.Group, () =>
        {
            RuleFor(c => c.Title)
                .NotEmpty()
                .WithMessage("title is required for group conversations")
                .MinimumLength(2)
                .WithMessage("title must be at least 2 characters")
                .MaximumLength(256)
                .WithMessage("title must not exceed 200 characters");
        });

        When(c => c.ConversationType == ConversationTypes.Direct, () =>
        {
            RuleFor(c => c.MemberIds)
                .Must(members => members != null && members.Count == 2)
                .WithMessage("direct conversations must have exactly 2 members");
        });

        RuleFor(c => c.Description)
            .MaximumLength(1024)
            .When(c => !string.IsNullOrWhiteSpace(c.Description))
            .WithMessage("description must not exceed 1000 characters");

        RuleFor(c => c.ImageUrl)
            .MaximumLength(512)
            .When(c => !string.IsNullOrWhiteSpace(c.ImageUrl))
            .WithMessage("image URL must not exceed 500 characters");

        RuleFor(c => c.MemberIds)
            .Must(members => members == null || members.Count > 0)
            .WithMessage("at least one member is required")
            .Must(members => members == null || members.Distinct().Count() == members.Count)
            .WithMessage("duplicate member IDs are not allowed");
    }
}

