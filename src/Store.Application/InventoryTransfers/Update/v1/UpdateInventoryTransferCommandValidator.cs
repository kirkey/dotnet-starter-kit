namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

public class UpdateInventoryTransferCommandValidator : AbstractValidator<UpdateInventoryTransferCommand>
{
    public UpdateInventoryTransferCommandValidator([FromKeyedServices("store:inventory-transfers")] IReadRepository<InventoryTransfer> readRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transfer ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Transfer name is required")
            .MaximumLength(100)
            .WithMessage("Transfer name must not exceed 100 characters");

        RuleFor(x => x.TransferNumber)
            .NotEmpty()
            .WithMessage("Transfer number is required")
            .MaximumLength(20)
            .WithMessage("Transfer number must not exceed 20 characters")
            .Matches(@"^[A-Z0-9]+$")
            .WithMessage("Transfer number must contain only uppercase letters and numbers");

        // Async uniqueness check for update (exclude current)
        RuleFor(x => x.TransferNumber).MustAsync(async (cmd, transferNumber, ct) =>
        {
            if (string.IsNullOrWhiteSpace(transferNumber)) return true;
            var existing = await readRepository.FirstOrDefaultAsync(new Specs.InventoryTransferByNumberSpec(transferNumber), ct).ConfigureAwait(false);
            return existing is null || existing.Id == cmd.Id;
        }).WithMessage("Another inventory transfer with the same TransferNumber already exists.");

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

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(status => new[] { "Pending", "Approved", "InTransit", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status must be one of: Pending, Approved, InTransit, Completed, Cancelled");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Transfer reason is required")
            .MaximumLength(200)
            .WithMessage("Transfer reason must not exceed 200 characters");
    }
}
