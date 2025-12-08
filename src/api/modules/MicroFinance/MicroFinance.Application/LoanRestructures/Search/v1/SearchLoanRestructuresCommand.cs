using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Search.v1;

/// <summary>
/// Command to search for loan restructures.
/// </summary>
public class SearchLoanRestructuresCommand : PaginationFilter, IRequest<PagedList<LoanRestructureSummaryResponse>>
{
    /// <summary>Filter by loan ID.</summary>
    public DefaultIdType? LoanId { get; set; }

    /// <summary>Filter by restructure number.</summary>
    public string? RestructureNumber { get; set; }

    /// <summary>Filter by restructure type.</summary>
    public string? RestructureType { get; set; }

    /// <summary>Filter by status.</summary>
    public string? Status { get; set; }

    /// <summary>Filter by request date from.</summary>
    public DateOnly? RequestDateFrom { get; set; }

    /// <summary>Filter by request date to.</summary>
    public DateOnly? RequestDateTo { get; set; }

    /// <summary>Filter by effective date from.</summary>
    public DateOnly? EffectiveDateFrom { get; set; }

    /// <summary>Filter by effective date to.</summary>
    public DateOnly? EffectiveDateTo { get; set; }
}

/// <summary>
/// Summary response for loan restructure search results.
/// </summary>
public sealed record LoanRestructureSummaryResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    string RestructureNumber,
    string RestructureType,
    DateOnly RequestDate,
    DateOnly? EffectiveDate,
    decimal OriginalPrincipal,
    decimal NewPrincipal,
    decimal OriginalInterestRate,
    decimal NewInterestRate,
    int OriginalRemainingTerm,
    int NewTerm,
    decimal WaivedAmount,
    string Status,
    string? ApprovedBy,
    DateTime? ApprovedAt
);
