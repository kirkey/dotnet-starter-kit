using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.GroupMemberships.UpdateRole.v1;

/// <summary>
/// Validator for UpdateMembershipRoleCommand.
/// </summary>
public class UpdateMembershipRoleCommandValidator : AbstractValidator<UpdateMembershipRoleCommand>
{
    public UpdateMembershipRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Membership ID is required.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .MaximumLength(GroupMembership.RoleMaxLength);
    }
}
