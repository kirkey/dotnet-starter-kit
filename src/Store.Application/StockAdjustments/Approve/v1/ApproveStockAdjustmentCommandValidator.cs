namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;

public class ApproveStockAdjustmentCommandValidator : AbstractValidator<ApproveStockAdjustmentCommand>
{
    public ApproveStockAdjustmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Adjustment Id is required");
        RuleFor(x => x.ApprovedBy).NotEmpty().WithMessage("ApprovedBy is required when approving");
    }
}

