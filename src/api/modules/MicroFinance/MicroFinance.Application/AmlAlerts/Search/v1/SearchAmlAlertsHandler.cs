using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Search.v1;

/// <summary>
/// Handler for searching AML alerts with pagination and filtering capabilities.
/// Implements CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public sealed class SearchAmlAlertsHandler(
    [FromKeyedServices("microfinance:amlalerts")] IReadRepository<AmlAlert> repository)
    : IRequestHandler<SearchAmlAlertsCommand, PagedList<AmlAlertResponse>>
{
    /// <summary>
    /// Handles the search request for AML alerts with filtering and pagination.
    /// </summary>
    /// <param name="request">The search command containing filter criteria and pagination settings.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A paginated list of AML alert responses matching the search criteria.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
    public async Task<PagedList<AmlAlertResponse>> Handle(SearchAmlAlertsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAmlAlertsSpec(request);

        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AmlAlertResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
