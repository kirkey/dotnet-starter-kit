namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Create.v1;

public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderCommandValidator([FromKeyedServices("store:purchase-orders")] IReadRepository<PurchaseOrder> readRepository)
    {
        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required")
            .MaximumLength(100).WithMessage("Order number must not exceed 100 characters")
            .Matches("^[A-Z0-9-]+$").WithMessage("Order number must contain only uppercase letters, numbers and hyphens");

        RuleFor(x => x.Status)
            .Must(PurchaseOrderStatus.IsAllowed)
            .WithMessage("Status is invalid");

        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Supplier is required");

        RuleFor(x => x.OrderDate)
            .NotEmpty().WithMessage("Order date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Order date cannot be in the far future");

        RuleFor(x => x.ExpectedDeliveryDate)
            .GreaterThanOrEqualTo(x => x.OrderDate)
            .When(x => x.ExpectedDeliveryDate.HasValue)
            .WithMessage("Expected delivery date must be on or after the order date");

        // Uniqueness check
        RuleFor(x => x.OrderNumber).MustAsync(async (orderNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(orderNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.PurchaseOrderByNumberSpec(orderNumber), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("A purchase order with the same OrderNumber already exists.");

        RuleFor(x => x.ContactPerson).MaximumLength(100);
        RuleFor(x => x.ContactPhone).MaximumLength(50);
        RuleFor(x => x.DeliveryAddress).MaximumLength(500);

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
