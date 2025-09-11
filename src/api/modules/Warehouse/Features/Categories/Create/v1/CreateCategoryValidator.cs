using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Categories.Create.v1;

public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Code)
            .NotEmpty()
            .MustAsync(async (code, ct) => await context.Categories.AllAsync(c => c.Code != code, ct))
            .WithMessage("Category code must be unique");
    }
}
