namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;

/// <summary>
/// Handler for searching lot numbers with filtering, sorting, and pagination support.
/// </summary>
/// <remarks>
/// Processes SearchLotNumbersCommand to retrieve a paginated list of lot numbers
/// based on search criteria such as lot code, item ID, expiration date ranges, and status.
/// Uses SearchLotNumbersSpec for building the query specification.
/// </remarks>
public sealed class SearchLotNumbersHandler(
    [FromKeyedServices("store:lotnumbers")] IReadRepository<LotNumber> repository)
    : IRequestHandler<SearchLotNumbersCommand, PagedList<LotNumberResponse>>
{
    /// <summary>
    /// Handles the search request for lot numbers.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of lot number responses.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<LotNumberResponse>> Handle(SearchLotNumbersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new Specs.SearchLotNumbersSpec(request);

        var lotNumbers = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var lotNumberResponses = lotNumbers.Select(lotNumber => new LotNumberResponse
        {
            Id = lotNumber.Id,
            LotNumber = lotNumber.LotCode, // Domain model uses LotCode
            ItemId = lotNumber.ItemId,
            ExpirationDate = lotNumber.ExpirationDate.HasValue ? DateOnly.FromDateTime(lotNumber.ExpirationDate.Value) : null, // Convert DateTime to DateOnly
            QuantityOnHand = lotNumber.QuantityRemaining, // Domain model uses QuantityRemaining
            Status = lotNumber.Status,
            ManufacturedDate = lotNumber.ManufactureDate.HasValue ? DateOnly.FromDateTime(lotNumber.ManufactureDate.Value) : null, // Convert DateTime to DateOnly
            ReceivedDate = lotNumber.ReceiptDate // Domain model uses ReceiptDate
        }).ToList();

        return new PagedList<LotNumberResponse>(lotNumberResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
