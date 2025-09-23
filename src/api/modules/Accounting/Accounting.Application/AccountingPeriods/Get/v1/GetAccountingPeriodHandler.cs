using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Get.v1;

/// <summary>
/// Handler for <see cref="GetAccountingPeriodQuery"/> that returns a detailed <see cref="AccountingPeriodResponse"/>.
/// Uses caching to avoid repeated repository lookups for frequently requested periods.
/// </summary>
public sealed class GetAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IReadRepository<AccountingPeriod> repository,
    ICacheService cache)
    : IRequestHandler<GetAccountingPeriodQuery, AccountingPeriodResponse>
{
    /// <summary>
    /// Handles the query by retrieving the accounting period from cache or repository and mapping it to the response DTO.
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
                var period = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (period == null) throw new AccountingPeriodNotFoundException(request.Id);
                return period.Adapt<AccountingPeriodResponse>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
