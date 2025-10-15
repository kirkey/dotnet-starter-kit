using Accounting.Application.AccountingPeriods.Responses;
using Accounting.Application.AccountingPeriods.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.AccountingPeriods.Get.v1;

/// <summary>
/// Handler for <see cref="GetAccountingPeriodQuery"/> that returns a detailed <see cref="AccountingPeriodResponse"/>.
/// Uses database-level projection for optimal performance with caching.
/// </summary>
public sealed class GetAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> repository,
    ICacheService cache)
    : IRequestHandler<GetAccountingPeriodQuery, AccountingPeriodResponse>
{
    /// <summary>
    /// Handles the query by retrieving the accounting period from cache or repository using specification projection.
    /// Throws <see cref="Accounting.Application.AccountingPeriods.Exceptions.AccountingPeriodNotFoundException"/>
    /// when the requested period cannot be found.
    /// </summary>
    /// <param name="request">The query containing the period identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A populated <see cref="AccountingPeriodResponse"/>.</returns>
    public async Task<AccountingPeriodResponse> Handle(GetAccountingPeriodQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"period:{request.Id}",
            async () =>
            {
                var spec = new GetAccountingPeriodSpec(request.Id);
                return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
                    ?? throw new AccountingPeriodNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
