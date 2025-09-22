namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Cancel.v1;

public class CancelInventoryTransferCommandValidator : AbstractValidator<CancelInventoryTransferCommand>
{
    public CancelInventoryTransferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory transfer id is required");
        RuleFor(x => x.Reason).MaximumLength(1000).When(x => !string.IsNullOrWhiteSpace(x.Reason)).WithMessage("Reason must not exceed 1000 characters");
    }
}

