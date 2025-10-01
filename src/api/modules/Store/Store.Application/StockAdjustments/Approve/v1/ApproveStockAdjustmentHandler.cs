using Store.Domain.Exceptions.StockAdjustment;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Approve.v1;

public sealed class ApproveStockAdjustmentHandler(
    ILogger<ApproveStockAdjustmentHandler> logger,
    [FromKeyedServices("store:stock-adjustments")] IRepository<StockAdjustment> repository)
    : IRequestHandler<ApproveStockAdjustmentCommand, ApproveStockAdjustmentResponse>
{
    public async Task<ApproveStockAdjustmentResponse> Handle(ApproveStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var sa = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = sa ?? throw new StockAdjustmentNotFoundException(request.Id);

        sa.Approve(request.ApprovedBy ?? throw new ArgumentException("ApprovedBy is required", nameof(request.ApprovedBy)));
        await repository.UpdateAsync(sa, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Approved stock adjustment {StockAdjustmentId} by {ApprovedBy}", sa.Id, sa.ApprovedBy);
        return new ApproveStockAdjustmentResponse(sa.Id, sa.IsApproved);
    }
}

