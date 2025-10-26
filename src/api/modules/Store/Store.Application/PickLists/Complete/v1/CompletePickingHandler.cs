using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;

/// <summary>
/// Handler for completing the picking process on a pick list.
/// </summary>
public sealed class CompletePickingHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<CompletePickingCommand, CompletePickingResponse>
{
    /// <summary>
    /// Handles the complete picking command.
    /// </summary>
    /// <param name="request">The complete picking command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The complete picking response.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<CompletePickingResponse> Handle(CompletePickingCommand request, CancellationToken cancellationToken)
    {
        // Load the pick list with its items collection only (no nested navigation properties)
        // to avoid EF Core tracking conflicts during write operations
        var spec = new GetPickListByIdWithItemsSpec(request.PickListId);
        var pickList = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.CompletePicking();

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new CompletePickingResponse
        {
            Success = true
        };
    }
}
