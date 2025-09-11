using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Sales.Update.v1;

public sealed class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

