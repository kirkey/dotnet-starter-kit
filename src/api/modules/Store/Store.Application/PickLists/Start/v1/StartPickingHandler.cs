using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;

/// <summary>
/// Handler for starting the picking process on a pick list.
/// </summary>
public sealed class StartPickingHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<StartPickingCommand, StartPickingResponse>
{
    /// <summary>
    /// Handles the start picking command.
    /// </summary>
    /// <param name="request">The start picking command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The start picking response.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<StartPickingResponse> Handle(StartPickingCommand request, CancellationToken cancellationToken)
    {
        // Load the pick list with its items collection only (no nested navigation properties)
        // to avoid EF Core tracking conflicts during write operations
        var spec = new GetPickListByIdWithItemsSpec(request.PickListId);
        var pickList = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.StartPicking();

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new StartPickingResponse
        {
            Success = true
        };
    }
}
