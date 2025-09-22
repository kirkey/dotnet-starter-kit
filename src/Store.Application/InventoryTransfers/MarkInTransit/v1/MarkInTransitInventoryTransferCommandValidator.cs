namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.MarkInTransit.v1;

public class MarkInTransitInventoryTransferCommandValidator : AbstractValidator<MarkInTransitInventoryTransferCommand>
{
    public MarkInTransitInventoryTransferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory transfer id is required");
    }
}
