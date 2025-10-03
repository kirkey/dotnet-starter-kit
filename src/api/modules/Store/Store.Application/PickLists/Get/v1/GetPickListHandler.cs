using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

public sealed class GetPickListHandler(
    [FromKeyedServices("store:picklists")] IReadRepository<PickList> repository)
    : IRequestHandler<GetPickListCommand, GetPickListResponse>
{
    public async Task<GetPickListResponse> Handle(GetPickListCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.FirstOrDefaultAsync(
            new GetPickListByIdSpec(request.PickListId), cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        return new GetPickListResponse
        {
            Id = pickList.Id,
            PickListNumber = pickList.PickListNumber,
            WarehouseId = pickList.WarehouseId,
            Status = pickList.Status,
            PickingType = pickList.PickingType,
            Priority = pickList.Priority,
            AssignedTo = pickList.AssignedTo,
            StartDate = pickList.StartDate,
            CompletedDate = pickList.CompletedDate,
            ExpectedCompletionDate = pickList.ExpectedCompletionDate,
            ReferenceNumber = pickList.ReferenceNumber,
            Notes = pickList.Notes,
            TotalLines = pickList.TotalLines,
            PickedLines = pickList.CompletedLines,
            CompletionPercentage = pickList.GetCompletionPercentage(),
            Items = pickList.Items.Select(item => new PickListItemDto
            {
                Id = item.Id,
                ItemId = item.ItemId,
                BinId = item.BinId,
                LotNumberId = item.LotNumberId,
                SerialNumberId = item.SerialNumberId,
                QuantityToPick = item.QuantityToPick,
                QuantityPicked = item.QuantityPicked,
                Status = item.Status,
                SequenceNumber = item.SequenceNumber,
                Notes = item.Notes,
                PickedDate = item.PickedDate
            }).ToList()
        };
    }
}
