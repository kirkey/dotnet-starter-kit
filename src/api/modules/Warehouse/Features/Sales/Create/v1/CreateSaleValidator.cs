using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Create.v1;

public sealed class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.SaleNumber)
            .NotEmpty()
            .MustAsync(async (no, ct) => await context.Sales.AllAsync(s => s.SaleNumber != no, ct))
            .WithMessage("Sale number must be unique");
        RuleFor(p => p.StoreId).NotEmpty();
    }
}

