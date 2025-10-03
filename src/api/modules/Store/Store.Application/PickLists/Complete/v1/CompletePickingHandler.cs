using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Complete.v1;

public sealed class CompletePickingHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<CompletePickingCommand, CompletePickingResponse>
{
    public async Task<CompletePickingResponse> Handle(CompletePickingCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.CompletePicking();

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new CompletePickingResponse
        {
            Success = true
        };
    }
}
