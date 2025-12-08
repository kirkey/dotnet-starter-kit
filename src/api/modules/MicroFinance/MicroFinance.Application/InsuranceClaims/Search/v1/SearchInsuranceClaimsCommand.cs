using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Search.v1;

/// <summary>
/// Command to search insurance claims.
/// </summary>
public class SearchInsuranceClaimsCommand : PaginationFilter, IRequest<PagedList<InsuranceClaimResponse>>
{
    /// <summary>
    /// Filter by insurance policy ID.
    /// </summary>
    public DefaultIdType? InsurancePolicyId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by claim type.
    /// </summary>
    public string? ClaimType { get; set; }
}

