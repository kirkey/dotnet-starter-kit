using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Start.v1;

public sealed class StartPickingHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<StartPickingCommand, StartPickingResponse>
{
    public async Task<StartPickingResponse> Handle(StartPickingCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.GetByIdAsync(request.PickListId, cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        pickList.StartPicking();

        await repository.UpdateAsync(pickList, cancellationToken).ConfigureAwait(false);

        return new StartPickingResponse
        {
            Success = true
        };
    }
}
