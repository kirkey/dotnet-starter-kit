using FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.Specs;
using Store.Domain.Exceptions.CycleCount;
using Store.Domain.Exceptions.CycleCountItem;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.AddItem.v1;

/// <summary>
/// Handles creation of a new <see cref="CycleCountItem"/> and updates the parent <see cref="CycleCount"/> totals.
/// </summary>
/// <remarks>
/// Validates that:
/// - The cycle count exists and is modifiable (not Completed/Cancelled).
/// - No duplicate item exists for the same (CycleCountId, ItemId).
/// On success, recalculates totals and persists changes.
/// Follows the Budget/BudgetDetail pattern for consistency.
/// </remarks>
public sealed class AddCycleCountItemHandler(
    ILogger<AddCycleCountItemHandler> logger,
    [FromKeyedServices("store:cycle-counts")] IRepository<CycleCount> cycleCountRepository,
    [FromKeyedServices("store:cycle-count-items")] IRepository<CycleCountItem> itemRepository,
    [FromKeyedServices("store:cycle-count-items")] IReadRepository<CycleCountItem> itemReadRepository)
    : IRequestHandler<AddCycleCountItemCommand, AddCycleCountItemResponse>
{
    public async Task<AddCycleCountItemResponse> Handle(AddCycleCountItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        // Ensure cycle count exists and is modifiable
        var cycleCount = await cycleCountRepository.GetByIdAsync(request.CycleCountId, cancellationToken).ConfigureAwait(false);
        _ = cycleCount ?? throw new CycleCountNotFoundException(request.CycleCountId);
        
        // Prevent modifications to completed or cancelled counts
        if (cycleCount.Status is "Completed" or "Cancelled")
        {
            throw new InvalidOperationException($"Cannot add items to a {cycleCount.Status} cycle count.");
        }
        
        // Prevent duplicate (CycleCountId, ItemId) - check in database
        var exists = await itemReadRepository.AnyAsync(
            new CycleCountItemByCycleCountAndItemSpec(request.CycleCountId, request.ItemId), 
            cancellationToken).ConfigureAwait(false);
        if (exists)
        {
            throw new DuplicateCycleCountItemException(request.CycleCountId, request.ItemId);
        }
        
        // Create the cycle count item independently
        var entity = CycleCountItem.Create(
            request.CycleCountId, 
            request.ItemId, 
            request.SystemQuantity, 
            request.CountedQuantity);
        
        await itemRepository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        
        // Recalculate totals and update cycle count (similar to Budget.SetTotals pattern)
        var items = await itemReadRepository.ListAsync(
            new CycleCountItemsByCycleCountIdSpec(request.CycleCountId), 
            cancellationToken).ConfigureAwait(false);
        
        var totalItems = items.Count;
        var itemsCountedCorrect = items.Count(i => i.IsAccurate());
        var itemsWithDiscrepancies = totalItems - itemsCountedCorrect;
        
        cycleCount.SetCounts(totalItems, itemsCountedCorrect, itemsWithDiscrepancies);
        
        await cycleCountRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation(
            "Added item {ItemId} to cycle count {CycleCountId} - total items now {Total}", 
            entity.Id, cycleCount.Id, totalItems);
        
        return new AddCycleCountItemResponse(entity.Id, cycleCount.Id);
    }
}

