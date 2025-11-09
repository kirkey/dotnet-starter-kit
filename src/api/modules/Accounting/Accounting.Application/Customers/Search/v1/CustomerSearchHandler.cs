namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Handler for paginated customer search.
/// Supports filtering by customer number, name, type, status, and other criteria with pagination.
/// </summary>
public sealed class CustomerSearchHandler(
    ILogger<CustomerSearchHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Customer> repository)
    : IRequestHandler<CustomerSearchRequest, PagedList<CustomerSearchResponse>>
{
    /// <summary>
    /// Handles the customer search request with pagination.
    /// </summary>
    /// <param name="request">The search request with filters and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of customer search results.</returns>
    public async Task<PagedList<CustomerSearchResponse>> Handle(CustomerSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new CustomerSearchSpecs(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Customer search completed: Page {Page}, {Count}/{Total} records",
            request.PageNumber,
            list.Count,
            totalCount);

        return new PagedList<CustomerSearchResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}

