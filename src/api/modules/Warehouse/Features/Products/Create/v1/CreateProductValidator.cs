using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Create.v1;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.SKU)
            .NotEmpty()
            .MustAsync(async (sku, ct) => await context.Products.AllAsync(p => p.SKU != sku, ct))
            .WithMessage("SKU must be unique");
    }
}

