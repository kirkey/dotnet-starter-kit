using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;

/// <summary>
/// Handler for deleting a pick list.
/// </summary>
public sealed class DeletePickListHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<DeletePickListCommand, DeletePickListResponse>
{
    /// <summary>
    /// Handles the delete pick list command.
    /// </summary>
    /// <param name="request">The delete command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The delete response.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<DeletePickListResponse> Handle(DeletePickListCommand request, CancellationToken cancellationToken)
    {
        // Load the pick list with its items collection only (no nested navigation properties)
        // to avoid EF Core tracking conflicts during write operations
        var spec = new GetPickListByIdWithItemsSpec(request.PickListId);
        var pickList = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        await repository.DeleteAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new DeletePickListResponse
        {
            Success = true
        };
    }
}
