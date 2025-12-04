using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.MemberGroups.AddMember.v1;

public sealed class AddMemberToGroupCommandValidator : AbstractValidator<AddMemberToGroupCommand>
{
    public AddMemberToGroupCommandValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty()
            .WithMessage("Group ID is required.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.Role)
            .MaximumLength(GroupMembership.RoleMaxLength)
            .Must(BeValidRole!)
            .When(x => !string.IsNullOrWhiteSpace(x.Role))
            .WithMessage($"Role must be one of: {GroupMembership.RoleMember}, {GroupMembership.RoleLeader}, {GroupMembership.RoleSecretary}, {GroupMembership.RoleTreasurer}.");

        RuleFor(x => x.Notes)
            .MaximumLength(GroupMembership.NotesMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage($"Notes must not exceed {GroupMembership.NotesMaxLength} characters.");
    }

    private static bool BeValidRole(string role) =>
        role is GroupMembership.RoleMember
            or GroupMembership.RoleLeader
            or GroupMembership.RoleSecretary
            or GroupMembership.RoleTreasurer;
}
