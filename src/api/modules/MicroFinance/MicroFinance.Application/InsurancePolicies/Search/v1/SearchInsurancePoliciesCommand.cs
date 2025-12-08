using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Search.v1;

/// <summary>
/// Command to search insurance policies.
/// </summary>
public class SearchInsurancePoliciesCommand : PaginationFilter, IRequest<PagedList<InsurancePolicyResponse>>
{
    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by product ID.
    /// </summary>
    public DefaultIdType? InsuranceProductId { get; set; }
}

