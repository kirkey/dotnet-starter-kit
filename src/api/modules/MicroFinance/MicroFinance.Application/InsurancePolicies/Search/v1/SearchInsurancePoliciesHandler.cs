using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Search.v1;

/// <summary>
/// Handler for searching insurance policies.
/// </summary>
public sealed class SearchInsurancePoliciesHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IReadRepository<InsurancePolicy> repository)
    : IRequestHandler<SearchInsurancePoliciesCommand, PagedList<InsurancePolicyResponse>>
{
    public async Task<PagedList<InsurancePolicyResponse>> Handle(SearchInsurancePoliciesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInsurancePoliciesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InsurancePolicyResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
