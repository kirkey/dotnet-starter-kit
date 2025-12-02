using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

/// <summary>
/// Validator for UpdatePurchaseOrderCommand.
/// Applies comprehensive validation rules for all purchase order properties.
/// </summary>
public class UpdatePurchaseOrderCommandValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
    public UpdatePurchaseOrderCommandValidator(
        [FromKeyedServices("store:purchase-orders")] IReadRepository<PurchaseOrder> readRepository,
        [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Purchase order ID is required.");

        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order number is required.")
            .MinimumLength(3).WithMessage("Order number must be at least 3 characters.")
            .MaximumLength(128).WithMessage("Order number must not exceed 100 characters.")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Order number must contain only uppercase letters, numbers, hyphens, and underscores.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(PurchaseOrderStatus.IsAllowed!)
            .WithMessage("Status is invalid. Allowed values are: Draft, Submitted, Approved, Sent, Received, Cancelled.");

        // Uniqueness check excluding current
        RuleFor(x => x.OrderNumber).MustAsync(async (cmd, orderNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(orderNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new PurchaseOrderByNumberSpec(orderNumber), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another purchase order with the same OrderNumber already exists.");

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Supplier is required.")
            .MustAsync(async (supplierId, ct) =>
            {
                if (supplierId == null || supplierId == default(DefaultIdType)) return false;
                var supplier = await supplierRepository.GetByIdAsync(supplierId.Value, ct).ConfigureAwait(false);
                return supplier != null;
            }).WithMessage("Supplier does not exist.");

        RuleFor(x => x.OrderDate)
            .NotNull().WithMessage("Order date is required.")
            .NotEmpty().WithMessage("Order date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Order date cannot be more than 1 day in the future.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddYears(-5)).WithMessage("Order date cannot be more than 5 years in the past.");

        RuleFor(x => x.ExpectedDeliveryDate)
            .GreaterThanOrEqualTo(x => x.OrderDate ?? DateTime.Now)
            .When(x => x.ExpectedDeliveryDate.HasValue)
            .WithMessage("Expected delivery date must be on or after the order date.");

        RuleFor(x => x.ActualDeliveryDate)
            .GreaterThanOrEqualTo(x => x.OrderDate ?? DateTime.Now)
            .When(x => x.ActualDeliveryDate.HasValue)
            .WithMessage("Actual delivery date must be on or after the order date.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .When(x => x.ActualDeliveryDate.HasValue)
            .WithMessage("Actual delivery date cannot be in the future.");

        RuleFor(x => x.ContactPerson)
            .MaximumLength(128).When(x => !string.IsNullOrWhiteSpace(x.ContactPerson))
            .WithMessage("Contact person must not exceed 100 characters.")
            .MinimumLength(2).When(x => !string.IsNullOrWhiteSpace(x.ContactPerson))
            .WithMessage("Contact person must be at least 2 characters.");

        RuleFor(x => x.ContactPhone)
            .MaximumLength(64).When(x => !string.IsNullOrWhiteSpace(x.ContactPhone))
            .WithMessage("Contact phone must not exceed 50 characters.")
            .MinimumLength(7).When(x => !string.IsNullOrWhiteSpace(x.ContactPhone))
            .WithMessage("Contact phone must be at least 7 characters.")
            .Matches(@"^[\d\s\-\+\(\)]+$").When(x => !string.IsNullOrWhiteSpace(x.ContactPhone))
            .WithMessage("Contact phone must contain only numbers, spaces, hyphens, plus signs, and parentheses.");

        RuleFor(x => x.DeliveryAddress)
            .MaximumLength(512).When(x => !string.IsNullOrWhiteSpace(x.DeliveryAddress))
            .WithMessage("Delivery address must not exceed 500 characters.")
            .MinimumLength(10).When(x => !string.IsNullOrWhiteSpace(x.DeliveryAddress))
            .WithMessage("Delivery address must be at least 10 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 2000 characters.");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0).When(x => x.TaxAmount.HasValue)
            .WithMessage("Tax amount must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(x => x.TaxAmount.HasValue)
            .WithMessage("Tax amount must not exceed 1,000,000.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0).When(x => x.DiscountAmount.HasValue)
            .WithMessage("Discount amount must be zero or greater.")
            .LessThanOrEqualTo(1000000).When(x => x.DiscountAmount.HasValue)
            .WithMessage("Discount amount must not exceed 1,000,000.");
    }
}
