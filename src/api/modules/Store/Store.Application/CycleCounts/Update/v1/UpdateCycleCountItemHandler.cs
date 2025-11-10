using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Update.v1;

/// <summary>
/// Handler for updating cycle count items.
/// Updates the counted quantity and notes for an item.
/// </summary>
public sealed class UpdateCycleCountItemHandler(
    IRepository<CycleCountItem> repository)
    : IRequestHandler<UpdateCycleCountItemCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCycleCountItemCommand command, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(command.Id, cancellationToken);
        
        if (item == null)
        {
            throw new NotFoundException($"Cycle count item with ID {command.Id} not found.");
        }

        // Record the count with the actual quantity
        item.RecordCount((int)command.ActualQuantity);

        // Update notes if provided
        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            item.Update(command.Notes);
        }

        await repository.UpdateAsync(item, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}

