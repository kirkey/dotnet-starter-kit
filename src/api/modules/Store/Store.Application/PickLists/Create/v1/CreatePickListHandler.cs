using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Create.v1;

public sealed class CreatePickListHandler(
    [FromKeyedServices("store:picklists")] IRepository<PickList> repository)
    : IRequestHandler<CreatePickListCommand, CreatePickListResponse>
{
    public async Task<CreatePickListResponse> Handle(CreatePickListCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate pick list number
        var existingPickList = await repository.FirstOrDefaultAsync(
            new PickListByNumberSpec(request.PickListNumber), cancellationToken).ConfigureAwait(false);

        if (existingPickList is not null)
        {
            throw new PickListAlreadyExistsException(request.PickListNumber);
        }

        var pickList = PickList.Create(
            request.PickListNumber,
            request.WarehouseId,
            request.PickingType,
            request.Priority,
            request.ReferenceNumber,
            request.Notes);

        await repository.AddAsync(pickList, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new CreatePickListResponse
        {
            Id = pickList.Id
        };
    }
}
