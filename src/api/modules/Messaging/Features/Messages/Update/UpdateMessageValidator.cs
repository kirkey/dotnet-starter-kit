namespace FSH.Starter.WebApi.Messaging.Features.Messages.Update;

public class UpdateMessageValidator : AbstractValidator<UpdateMessageCommand>
{
    public UpdateMessageValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("message id is required");

        RuleFor(m => m.Content)
            .NotEmpty()
            .WithMessage("message content is required")
            .MaximumLength(5000)
            .WithMessage("message content must not exceed 5000 characters");
    }
}

