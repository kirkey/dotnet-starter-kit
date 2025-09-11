using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Create.v1;

public sealed class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.OrderNumber)
            .NotEmpty()
            .MustAsync(async (no, ct) => await context.PurchaseOrders.AllAsync(x => x.OrderNumber != no, ct))
            .WithMessage("OrderNumber must be unique");
        RuleFor(p => p.SupplierId).NotEmpty();
        RuleFor(p => p.WarehouseId).NotEmpty();
    }
}

