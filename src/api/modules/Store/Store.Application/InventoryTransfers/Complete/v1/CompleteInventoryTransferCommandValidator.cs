namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Complete.v1;

public class CompleteInventoryTransferCommandValidator : AbstractValidator<CompleteInventoryTransferCommand>
{
    public CompleteInventoryTransferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory transfer id is required");
        RuleFor(x => x.ActualArrival).NotEqual(default(DateTime)).WithMessage("Actual arrival date is required");
    }
}

