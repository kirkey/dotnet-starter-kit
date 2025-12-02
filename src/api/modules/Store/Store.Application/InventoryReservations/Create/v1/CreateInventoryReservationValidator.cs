namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Create.v1;

public class CreateInventoryReservationValidator : AbstractValidator<CreateInventoryReservationCommand>
{
    public CreateInventoryReservationValidator()
    {
        RuleFor(x => x.ReservationNumber)
            .NotEmpty().WithMessage("Reservation number is required.")
            .MaximumLength(128).WithMessage("Reservation number must not exceed 100 characters.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item ID is required.");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required.");

        RuleFor(x => x.QuantityReserved)
            .GreaterThan(0).WithMessage("Quantity reserved must be positive.");

        RuleFor(x => x.ReservationType)
            .NotEmpty().WithMessage("Reservation type is required.")
            .Must(type => new[] { "Order", "Transfer", "Production", "Assembly", "Other" }
                .Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Reservation type must be one of: Order, Transfer, Production, Assembly, Other.");

        RuleFor(x => x.ReferenceNumber)
            .MaximumLength(128).WithMessage("Reference number must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReferenceNumber));

        RuleFor(x => x.ReservedBy)
            .MaximumLength(128).WithMessage("Reserved by must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ReservedBy));

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.")
            .When(x => x.ExpirationDate.HasValue);

        RuleFor(x => x.Name)
            .MaximumLength(1024).When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
