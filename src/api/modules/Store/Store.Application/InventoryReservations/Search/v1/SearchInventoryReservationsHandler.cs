using FSH.Starter.WebApi.Store.Application.InventoryReservations.Specs;

namespace FSH.Starter.WebApi.Store.Application.InventoryReservations.Search.v1;

public class SearchInventoryReservationsHandler(
    [FromKeyedServices("store:inventoryreservations")] IReadRepository<InventoryReservation> repository)
    : IRequestHandler<SearchInventoryReservationsCommand, PagedList<InventoryReservationResponse>>
{
    public async Task<PagedList<InventoryReservationResponse>> Handle(SearchInventoryReservationsCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchInventoryReservationsSpec(request);

        var reservations = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        return new PagedList<InventoryReservationDto>(reservations, request.PageNumber, request.PageSize, totalCount);
    }
}
