namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

/// <summary>
/// Validator for UpdateInventoryTransferCommand with strict validation rules.
/// Ensures all required fields are present and valid before updating an inventory transfer.
/// </summary>
public class UpdateInventoryTransferCommandValidator : AbstractValidator<UpdateInventoryTransferCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateInventoryTransferCommandValidator"/> class.
    /// </summary>
    public UpdateInventoryTransferCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Inventory transfer ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(256)
            .WithMessage("Name must not exceed 200 characters.")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(1024)
            .WithMessage("Description must not exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.TransferNumber)
            .NotEmpty()
            .WithMessage("Transfer number is required.")
            .MaximumLength(64)
            .WithMessage("Transfer number must not exceed 50 characters.")
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("Transfer number must contain only uppercase letters, numbers, and hyphens.");

        RuleFor(x => x.TransferType)
            .NotEmpty()
            .WithMessage("Transfer type is required.")
            .Must(x => new[] { "Standard", "Emergency", "Replenishment", "Return" }.Contains(x))
            .WithMessage("Transfer type must be one of: Standard, Emergency, Replenishment, Return.");

        RuleFor(x => x.FromWarehouseId)
            .NotEmpty()
            .WithMessage("Source warehouse is required.");

        RuleFor(x => x.ToWarehouseId)
            .NotEmpty()
            .WithMessage("Destination warehouse is required.");

        RuleFor(x => x)
            .Must(x => x.FromWarehouseId != x.ToWarehouseId)
            .WithMessage("Source and destination warehouses must be different.")
            .When(x => x.FromWarehouseId != DefaultIdType.Empty && x.ToWarehouseId != DefaultIdType.Empty);

        RuleFor(x => x.TransferDate)
            .NotEmpty()
            .WithMessage("Transfer date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("Transfer date cannot be more than 30 days in the future.");

        RuleFor(x => x.ExpectedArrivalDate)
            .GreaterThan(x => x.TransferDate)
            .WithMessage("Expected arrival date must be after transfer date.")
            .When(x => x.ExpectedArrivalDate.HasValue);

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required.")
            .Must(x => new[] { "Pending", "Draft", "Approved", "InTransit", "Completed", "Cancelled" }.Contains(x))
            .WithMessage("Status must be one of: Pending, Draft, Approved, InTransit, Completed, Cancelled.");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority is required.")
            .Must(x => new[] { "Low", "Normal", "High", "Urgent" }.Contains(x))
            .WithMessage("Priority must be one of: Low, Normal, High, Urgent.");

        RuleFor(x => x.TransportMethod)
            .MaximumLength(128)
            .WithMessage("Transport method must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.TransportMethod));

        RuleFor(x => x.TrackingNumber)
            .MaximumLength(128)
            .WithMessage("Tracking number must not exceed 100 characters.")
            .When(x => !string.IsNullOrEmpty(x.TrackingNumber));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes must not exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required.")
            .MaximumLength(512)
            .WithMessage("Reason must not exceed 500 characters.")
            .MinimumLength(5)
            .WithMessage("Reason must be at least 5 characters long.");
    }
}

