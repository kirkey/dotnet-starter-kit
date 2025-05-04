using FluentValidation;
using FSH.Starter.WebApi.App.Persistence;

namespace FSH.Starter.WebApi.App.Features.Update.v1;

public class GroupUpdateValidator : AbstractValidator<GroupUpdateCommand>
{
    public GroupUpdateValidator(AppDbContext context)
    {
        RuleFor(p => p.Application).NotEmpty();
        RuleFor(p => p.Parent).NotEmpty();
        RuleFor(p => p.Code).NotEmpty();
        RuleFor(p => p.Name).NotEmpty();
    }
}
