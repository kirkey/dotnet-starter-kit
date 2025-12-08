using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Specifications;
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

        var spec = new EntitiesByPaginationFilterSpec<InsuranceClaim, InsuranceClaimResponse>(
            new PaginationFilter
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Keyword = request.Keyword,
                OrderBy = request.OrderBy
            });

        return await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken)
            .ConfigureAwait(false);
    }
}

