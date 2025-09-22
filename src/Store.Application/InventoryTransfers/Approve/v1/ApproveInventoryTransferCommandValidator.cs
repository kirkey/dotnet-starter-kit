namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Approve.v1;

public class ApproveInventoryTransferCommandValidator : AbstractValidator<ApproveInventoryTransferCommand>
{
    public ApproveInventoryTransferCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory transfer id is required");
        RuleFor(x => x.ApprovedBy).NotEmpty().MaximumLength(100).WithMessage("ApprovedBy is required and must not exceed 100 characters");
    }
}

