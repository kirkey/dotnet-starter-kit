using Store.Domain.Exceptions.CycleCount;
using Store.Domain.Exceptions.CycleCountItem;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.RecordCount.v1;

/// <summary>
/// Handler for recording counted quantities for cycle count items.
/// This is the core operation during the counting phase.
/// </summary>
public sealed class RecordCycleCountItemHandler(
    ILogger<RecordCycleCountItemHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> repository)
    : IRequestHandler<RecordCycleCountItemCommand, RecordCycleCountItemResponse>
{
    public async Task<RecordCycleCountItemResponse> Handle(RecordCycleCountItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Load cycle count with items
        var cycleCount = await repository.GetByIdAsync(request.CycleCountId, cancellationToken).ConfigureAwait(false);
        _ = cycleCount ?? throw new CycleCountNotFoundException(request.CycleCountId);

        // Validate cycle count is in progress
        if (cycleCount.Status != "InProgress")
        {
            throw new InvalidCycleCountStatusException(cycleCount.Status, 
                "Cycle count must be in 'InProgress' status to record counts");
        }

        // Find the specific item
        var item = cycleCount.Items.FirstOrDefault(i => i.Id == request.CycleCountItemId);
        if (item is null)
        {
            throw new CycleCountItemNotFoundException(request.CycleCountItemId);
        }

        // Record the count
        item.RecordCount(request.CountedQuantity, request.CountedBy);
        
        // Update notes if provided
        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            item.Update(request.Notes);
        }

        // Check if variance is significant and mark for recount if needed
        if (item.HasSignificantVariance(threshold: 10))
        {
            item.MarkForRecount($"Significant variance detected: {item.VarianceQuantity}");
            logger.LogWarning(
                "Significant variance detected for cycle count item {ItemId} in cycle count {CycleCountId}. Variance: {Variance}",
                item.Id, cycleCount.Id, item.VarianceQuantity);
        }

        await repository.UpdateAsync(cycleCount, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Recorded count for item {ItemId} in cycle count {CycleCountId}. System: {System}, Counted: {Counted}, Variance: {Variance}",
            item.Id, cycleCount.Id, item.SystemQuantity, item.CountedQuantity, item.VarianceQuantity);

        return new RecordCycleCountItemResponse(
            item.Id,
            cycleCount.Id,
            item.SystemQuantity,
            item.CountedQuantity ?? 0,
            item.VarianceQuantity ?? 0,
            item.IsAccurate(),
            item.RequiresRecount);
    }
}

