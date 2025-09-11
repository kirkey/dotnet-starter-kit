using FluentValidation;
using FSH.Starter.WebApi.Warehouse.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Create.v1;

public sealed class CreateSupplierValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierValidator(WarehouseDbContext context)
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Code)
            .NotEmpty()
            .MustAsync(async (code, ct) => await context.Suppliers.AllAsync(s => s.Code != code, ct))
            .WithMessage("Supplier code must be unique");
        RuleFor(p => p.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
