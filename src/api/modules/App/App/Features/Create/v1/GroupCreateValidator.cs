using FluentValidation;
using FSH.Starter.WebApi.App.Persistence;

namespace FSH.Starter.WebApi.App.Features.Create.v1;

public class GroupCreateValidator : AbstractValidator<GroupCreateCommand>
{
    public GroupCreateValidator(AppDbContext context)
    {
        RuleFor(p => p.Application).NotEmpty();
        RuleFor(p => p.Parent).NotEmpty();
        RuleFor(p => p.Code).NotEmpty();
        RuleFor(p => p.Name).NotEmpty();
    }
}
