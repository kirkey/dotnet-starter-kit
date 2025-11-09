using FSH.Starter.WebApi.Store.Application.InventoryTransfers.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;

public class CreateInventoryTransferCommandValidator : AbstractValidator<CreateInventoryTransferCommand>
{
    public CreateInventoryTransferCommandValidator([FromKeyedServices("store:inventory-transfers")] IReadRepository<InventoryTransfer> readRepository)
    {
        RuleFor(x => x.TransferNumber)
            .NotEmpty()
            .WithMessage("Transfer number is required")
            .MaximumLength(20)
            .WithMessage("Transfer number must not exceed 20 characters")
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Transfer number must contain only uppercase letters and numbers");

        // Async uniqueness check
        RuleFor(x => x.TransferNumber).MustAsync(async (transferNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(transferNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new InventoryTransferByNumberSpec(transferNumber), ct).ConfigureAwait(false);
            return existing is null;
        }).WithMessage("An inventory transfer with the same TransferNumber already exists.");

        RuleFor(x => x.FromWarehouseId)
            .NotEmpty()
            .WithMessage("From warehouse ID is required");

        RuleFor(x => x.ToWarehouseId)
            .NotEmpty()
            .WithMessage("To warehouse ID is required")
            .NotEqual(x => x.FromWarehouseId)
            .WithMessage("To warehouse must be different from from warehouse");

        RuleFor(x => x.TransferDate)
            .NotEmpty()
            .WithMessage("Transfer date is required")
            .GreaterThanOrEqualTo(DateTime.Today.AddDays(-30))
            .WithMessage("Transfer date cannot be more than 30 days in the past");

        RuleFor(x => x.ExpectedArrivalDate)
            .GreaterThanOrEqualTo(x => x.TransferDate)
            .When(x => x.ExpectedArrivalDate.HasValue)
            .WithMessage("Expected arrival date must be on or after transfer date");

        RuleFor(x => x.TransferType)
            .NotEmpty()
            .WithMessage("Transfer type is required")
            .Must(type => new[] { "Standard", "Emergency", "Replenishment", "Return" }.Contains(type))
            .WithMessage("Transfer type must be one of: Standard, Emergency, Replenishment, Return");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority is required")
            .Must(p => new[] { "Low", "Normal", "High", "Urgent" }.Contains(p))
            .WithMessage("Priority must be one of: Low, Normal, High, Urgent");

        RuleFor(x => x.TransportMethod)
            .MaximumLength(100)
            .WithMessage("Transport method must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.TransportMethod));

        RuleFor(x => x.Name)
            .MaximumLength(1024)
            .WithMessage("Name must not exceed 1024 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must not exceed 2048 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.RequestedBy)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.RequestedBy));
    }
}
