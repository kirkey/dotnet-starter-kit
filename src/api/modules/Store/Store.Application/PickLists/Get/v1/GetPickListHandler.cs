using Store.Domain.Exceptions.PickList;

namespace FSH.Starter.WebApi.Store.Application.PickLists.Get.v1;

/// <summary>
/// Handler for getting a pick list by ID with all its details.
/// </summary>
public sealed class GetPickListHandler(
    [FromKeyedServices("store:picklists")] IReadRepository<PickList> repository)
    : IRequestHandler<GetPickListCommand, GetPickListResponse>
{
    /// <summary>
    /// Handles the get pick list command.
    /// </summary>
    /// <param name="request">The get pick list command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The pick list response with all details.</returns>
    /// <exception cref="PickListNotFoundException">Thrown when the pick list is not found.</exception>
    public async Task<GetPickListResponse> Handle(GetPickListCommand request, CancellationToken cancellationToken)
    {
        var pickList = await repository.FirstOrDefaultAsync(
            new GetPickListByIdSpec(request.PickListId), cancellationToken).ConfigureAwait(false)
            ?? throw new PickListNotFoundException(request.PickListId);

        return new GetPickListResponse
        {
            Id = pickList.Id,
            Name = pickList.Name,
            Description = pickList.Description,
            PickListNumber = pickList.PickListNumber,
            WarehouseId = pickList.WarehouseId,
            WarehouseName = pickList.Warehouse.Name,
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
                ItemName = item.Item.Name,
                BinId = item.BinId,
                BinName = item.Bin?.Name ?? string.Empty,
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
