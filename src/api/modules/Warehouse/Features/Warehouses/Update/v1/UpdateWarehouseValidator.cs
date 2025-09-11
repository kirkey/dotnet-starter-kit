using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Warehouses.Update.v1;

public sealed class UpdateWarehouseValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

