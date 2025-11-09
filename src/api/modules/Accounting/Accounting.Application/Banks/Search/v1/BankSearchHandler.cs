using Accounting.Application.Banks.Get.v1;

namespace Accounting.Application.Banks.Search.v1;

/// <summary>
/// Handler for searching banks with filtering and pagination.
/// </summary>
public sealed class BankSearchHandler(
    [FromKeyedServices("accounting:banks")] IReadRepository<Bank> repository)
    : IRequestHandler<BankSearchRequest, PagedList<BankResponse>>
{
    /// <summary>
    /// Handles the search operation for banks.
    /// </summary>
    /// <param name="request">The search request containing filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>Paged list of banks matching the search criteria.</returns>
    public async Task<PagedList<BankResponse>> Handle(BankSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new BankSearchSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<BankResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

