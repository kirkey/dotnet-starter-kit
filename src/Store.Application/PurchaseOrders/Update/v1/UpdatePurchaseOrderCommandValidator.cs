namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

public class UpdatePurchaseOrderCommandValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
    public UpdatePurchaseOrderCommandValidator([FromKeyedServices("store:purchase-orders")] IReadRepository<PurchaseOrder> readRepository,
                                                [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Purchase order ID is required");

        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(50).WithMessage("Order number must not exceed 50 characters")
            .Matches("^[A-Z0-9-]+$").WithMessage("Order number must contain only uppercase letters, numbers and hyphens");

        // Uniqueness check excluding current
        RuleFor(x => x.OrderNumber).MustAsync(async (cmd, orderNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(orderNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.PurchaseOrderByNumberSpec(orderNumber), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another purchase order with the same OrderNumber already exists.");

        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Supplier is required");

        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("Order date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Order date cannot be in the far future");

        RuleFor(x => x.ExpectedDeliveryDate)
            .GreaterThanOrEqualTo(x => x.OrderDate)
            .When(x => x.ExpectedDeliveryDate.HasValue)
            .WithMessage("Expected delivery date must be on or after the order date");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(50).WithMessage("Contact phone must not exceed 50 characters");

        RuleFor(x => x.DeliveryAddress)
            .MaximumLength(500).WithMessage("Delivery address must not exceed 500 characters");
    }
}
