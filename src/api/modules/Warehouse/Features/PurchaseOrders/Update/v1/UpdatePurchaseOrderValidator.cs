using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Update.v1;

public sealed class UpdatePurchaseOrderValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
    public UpdatePurchaseOrderValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}

