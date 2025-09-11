using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Suppliers.Update.v1;

public sealed class UpdateSupplierValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

