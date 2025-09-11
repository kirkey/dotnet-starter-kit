using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Stores.Create.v1;

public sealed class CreateStoreValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Code)
            .NotEmpty()
            .MustAsync(async (code, ct) => await context.Stores.AllAsync(s => s.Code != code, ct))
            .WithMessage("Store code must be unique");
    }
}

