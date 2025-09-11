using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Transfers.Create.v1;

public sealed class CreateTransferValidator : AbstractValidator<CreateTransferCommand>
{
    public CreateTransferValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.TransferNumber)
            .NotEmpty()
            .MustAsync(async (no, ct) => await context.StoreTransfers.AllAsync(t => t.TransferNumber != no, ct))
            .WithMessage("Transfer number must be unique");
        RuleFor(p => p.FromWarehouseId).NotEmpty();
        RuleFor(p => p.ToStoreId).NotEmpty();
    }
}

