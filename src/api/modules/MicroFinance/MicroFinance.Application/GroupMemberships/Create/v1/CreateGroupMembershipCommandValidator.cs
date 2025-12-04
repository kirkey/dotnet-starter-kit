using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.Create.v1;

public sealed class CreateGroupMembershipCommandValidator : AbstractValidator<CreateGroupMembershipCommand>
{
    public CreateGroupMembershipCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Group ID is required.");

        RuleFor(x => x.Role)
            .MaximumLength(32)
            .When(x => !string.IsNullOrWhiteSpace(x.Role))
            .WithMessage("Role must not exceed 32 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 2048 characters.");
    }
}
