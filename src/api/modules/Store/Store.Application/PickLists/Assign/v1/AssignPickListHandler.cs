using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Assign.v1;

public sealed class AssignPickListHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<AssignPickListCommand, AssignPickListResponse>
{
    public async Task<AssignPickListResponse> Handle(AssignPickListCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.AssignToPicker(request.AssignedTo);

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new AssignPickListResponse
        {
            Success = true
        };
    }
}
