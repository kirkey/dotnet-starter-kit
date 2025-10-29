namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;

/// <summary>
/// Validator for AddMemberCommand.
/// </summary>
public class AddMemberValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberValidator()
    {
        RuleFor(c => c.ConversationId)
            .NotEmpty()
            .WithMessage("conversation id is required");

        RuleFor(c => c.UserId)
            .NotEmpty()
            .WithMessage("user id is required");

        RuleFor(c => c.Role)
            .NotEmpty()
            .WithMessage("role is required")
            .Must(role => role == MemberRoles.Admin || role == MemberRoles.Member)
            .WithMessage("role must be either 'admin' or 'member'");
    }
}

