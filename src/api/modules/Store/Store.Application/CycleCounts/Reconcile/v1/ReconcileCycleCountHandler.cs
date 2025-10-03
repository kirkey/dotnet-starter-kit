using Store.Domain.Exceptions.CycleCount;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Reconcile.v1;

public sealed class ReconcileCycleCountHandler(
    ILogger<ReconcileCycleCountHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IReadRepository<CycleCount> readRepository,
    [FromKeyedServices("store:items")] IReadRepository<Item> itemRepository,
    [FromKeyedServices("store:stock-adjustments")] IRepository<StockAdjustment> adjustmentRepository)
    : IRequestHandler<ReconcileCycleCountCommand, ReconcileCycleCountResponse>
{
    public async Task<ReconcileCycleCountResponse> Handle(ReconcileCycleCountCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var cc = await readRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = cc ?? throw new CycleCountNotFoundException(request.Id);
        if (cc.Status != "Completed")
            throw new InvalidCycleCountStatusException(cc.Status);

        var discrepancies = cc.Items
            .Where(i => !i.IsAccurate())
            .Select(i => new CycleCountDiscrepancy(i.ItemId, i.SystemQuantity, i.CountedQuantity ?? 0, (i.CountedQuantity ?? 0) - i.SystemQuantity))
            .ToList();

        // Create StockAdjustment records for each discrepancy
        foreach (var d in discrepancies)
        {
            var adjustmentType = d.Difference > 0 ? "Increase" : "Decrease";
            var adjustmentQuantity = Math.Abs(d.Difference);

            // Obtain unit cost from item repository (fall back to 0)
            decimal unitCost = 0m;
            try
            {
                var item = await itemRepository.GetByIdAsync(request.ItemId, cancellationToken).ConfigureAwait(false);
                if (item is not null) unitCost = item.Cost;
            }
            catch
            {
                unitCost = 0m;
            }

            var adjustmentNumber = $"SA-{DefaultIdType.NewGuid().ToString().Split('-').First()}";

            var adjustment = StockAdjustment.Create(
                adjustmentNumber,
                d.GroceryItemId,
                cc.WarehouseId,
                cc.WarehouseLocationId,
                DateTime.UtcNow,
                adjustmentType,
                "Cycle count reconciliation",
                d.SystemQuantity,
                adjustmentQuantity,
                unitCost,
                reference: null,
                notes: $"CycleCount:{cc.Id}");

            await adjustmentRepository.AddAsync(adjustment, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("created stock adjustment {AdjustmentId} for cycle count {CycleCountId}", adjustment.Id, cc.Id);
        }

        var readOnly = discrepancies.AsReadOnly();
        logger.LogInformation("reconciled cycle count {CycleCountId} with {Count} discrepancies", cc.Id, readOnly.Count);
        return new ReconcileCycleCountResponse(cc.Id, readOnly);
    }
}
