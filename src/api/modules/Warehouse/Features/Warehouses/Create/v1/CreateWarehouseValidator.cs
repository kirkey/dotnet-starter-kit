using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Create.v1;

public sealed class CreateWarehouseValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Code)
            .NotEmpty()
            .MustAsync(async (code, ct) => await context.Warehouses.AllAsync(w => w.Code != code, ct))
            .WithMessage("Warehouse code must be unique");
    }
}

