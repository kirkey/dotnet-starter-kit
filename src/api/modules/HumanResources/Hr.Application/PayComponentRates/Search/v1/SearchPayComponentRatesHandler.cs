namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Search.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;
using Specifications;

/// <summary>
/// Handler for searching pay component rates with filters and pagination.
/// </summary>
public sealed class SearchPayComponentRatesHandler(
    [FromKeyedServices("hr:paycomponentrates")] IReadRepository<PayComponentRate> repository)
    : IRequestHandler<SearchPayComponentRatesRequest, PagedList<PayComponentRateResponse>>
{
    public async Task<PagedList<PayComponentRateResponse>> Handle(
        SearchPayComponentRatesRequest request,
        CancellationToken cancellationToken)
    {
        var spec = new SearchPayComponentRatesSpec(request);
        var rates = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responses = rates.Select(rate => new PayComponentRateResponse(
            rate.Id,
            rate.PayComponentId,
            rate.MinAmount,
            rate.MaxAmount,
            rate.EmployeeRate,
            rate.EmployerRate,
            rate.AdditionalEmployerRate,
            rate.EmployeeAmount,
            rate.EmployerAmount,
            rate.TaxRate,
            rate.BaseAmount,
            rate.ExcessRate,
            rate.Year,
            null, // EffectiveStartDate
            null, // EffectiveEndDate
            rate.IsActive,
            null  // Description
        )).ToList();

        return new PagedList<PayComponentRateResponse>(
            responses,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

