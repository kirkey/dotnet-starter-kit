using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Search.v1;

/// <summary>
/// Handler for searching insurance claims.
/// </summary>
public sealed class SearchInsuranceClaimsHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IReadRepository<InsuranceClaim> repository)
    : IRequestHandler<SearchInsuranceClaimsCommand, PagedList<InsuranceClaimResponse>>
{
    public async Task<PagedList<InsuranceClaimResponse>> Handle(SearchInsuranceClaimsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInsuranceClaimsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InsuranceClaimResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

