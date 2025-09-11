using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Update.v1;

public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

