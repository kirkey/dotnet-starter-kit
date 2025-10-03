using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Delete.v1;

public sealed class DeletePickListHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<DeletePickListCommand, DeletePickListResponse>
{
    public async Task<DeletePickListResponse> Handle(DeletePickListCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        await repository.DeleteAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new DeletePickListResponse
        {
            Success = true
        };
    }
}
