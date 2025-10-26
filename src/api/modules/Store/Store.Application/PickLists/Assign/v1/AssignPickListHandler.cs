using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;

/// <summary>
/// Handler for assigning a pick list to a picker.
/// </summary>
public sealed class AssignPickListHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<AssignPickListCommand, AssignPickListResponse>
{
    /// <summary>
    /// Handles the assign pick list command.
    /// </summary>
    /// <param name="request">The assign command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The assign response.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<AssignPickListResponse> Handle(AssignPickListCommand request, CancellationToken cancellationToken)
    {
        // Load the pick list with its items collection only (no nested navigation properties)
        // to avoid EF Core tracking conflicts during write operations
        var spec = new GetPickListByIdWithItemsSpec(request.PickListId);
        var pickList = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.AssignToPicker(request.AssignedTo);

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new AssignPickListResponse
        {
            Success = true
        };
    }
}
