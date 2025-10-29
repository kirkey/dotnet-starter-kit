namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Update;

/// <summary>
/// Validator for UpdateConversationCommand with strict validation rules.
/// </summary>
public class UpdateConversationValidator : AbstractValidator<UpdateConversationCommand>
{
    public UpdateConversationValidator(MessagingDbContext context)
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("conversation id is required");

        RuleFor(c => c.Title)
            .MinimumLength(2)
            .When(c => !string.IsNullOrWhiteSpace(c.Title))
            .WithMessage("title must be at least 2 characters")
            .MaximumLength(200)
            .When(c => !string.IsNullOrWhiteSpace(c.Title))
            .WithMessage("title must not exceed 200 characters");

        RuleFor(c => c.Description)
            .MaximumLength(1000)
            .When(c => !string.IsNullOrWhiteSpace(c.Description))
            .WithMessage("description must not exceed 1000 characters");

        RuleFor(c => c.ImageUrl)
            .MaximumLength(500)
            .When(c => !string.IsNullOrWhiteSpace(c.ImageUrl))
            .WithMessage("image URL must not exceed 500 characters");
    }
}

