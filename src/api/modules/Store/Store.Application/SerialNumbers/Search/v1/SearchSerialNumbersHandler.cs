using FSH.Starter.WebApi.Store.Application.SerialNumbers.Specs;

namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Search.v1;

/// <summary>
/// Handler for searching serial numbers with filtering, sorting, and pagination support.
/// </summary>
/// <remarks>
/// Processes SearchSerialNumbersCommand to retrieve a paginated list of serial numbers
/// based on search criteria such as serial number value, item ID, warehouse ID, status, and date ranges.
/// Uses SearchSerialNumbersSpec for building the query specification.
/// </remarks>
public sealed class SearchSerialNumbersHandler(
    [FromKeyedServices("store:serialnumbers")] IReadRepository<SerialNumber> repository)
    : IRequestHandler<SearchSerialNumbersCommand, PagedList<SerialNumberResponse>>
{
    /// <summary>
    /// Handles the search request for serial numbers.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of serial number responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<SerialNumberResponse>> Handle(SearchSerialNumbersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSerialNumbersSpec(request);

        var serialNumbers = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var serialNumberResponses = serialNumbers.Select(sn => new SerialNumberResponse
        {
            Id = sn.Id,
            SerialNumberValue = sn.SerialNumberValue,
            ItemId = sn.ItemId,
            WarehouseId = sn.WarehouseId,
            WarehouseLocationId = sn.WarehouseLocationId,
            BinId = sn.BinId,
            LotNumberId = sn.LotNumberId,
            Status = sn.Status,
            ReceiptDate = sn.ReceiptDate, // Matches the domain model property name
            ManufactureDate = sn.ManufactureDate,
            WarrantyExpirationDate = sn.WarrantyExpirationDate,
            ExternalReference = sn.ExternalReference,
            CreatedOn = sn.CreatedOn // From AuditableEntity base class
        }).ToList();

        return new PagedList<SerialNumberResponse>(serialNumberResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
