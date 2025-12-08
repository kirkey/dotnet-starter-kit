using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Search.v1;

/// <summary>
/// Command to search for loan write-offs.
/// </summary>
public class SearchLoanWriteOffsCommand : PaginationFilter, IRequest<PagedList<LoanWriteOffSummaryResponse>>
{
    /// <summary>Filter by loan ID.</summary>
    public DefaultIdType? LoanId { get; set; }

    /// <summary>Filter by write-off number.</summary>
    public string? WriteOffNumber { get; set; }

    /// <summary>Filter by write-off type.</summary>
    public string? WriteOffType { get; set; }

    /// <summary>Filter by status.</summary>
    public string? Status { get; set; }

    /// <summary>Filter by request date from.</summary>
    public DateOnly? RequestDateFrom { get; set; }

    /// <summary>Filter by request date to.</summary>
    public DateOnly? RequestDateTo { get; set; }

    /// <summary>Filter by write-off date from.</summary>
    public DateOnly? WriteOffDateFrom { get; set; }

    /// <summary>Filter by write-off date to.</summary>
    public DateOnly? WriteOffDateTo { get; set; }

    /// <summary>Filter by minimum total write-off amount.</summary>
    public decimal? MinTotalWriteOff { get; set; }

    /// <summary>Filter by maximum total write-off amount.</summary>
    public decimal? MaxTotalWriteOff { get; set; }
}

/// <summary>
/// Summary response for loan write-off search results.
/// </summary>
public sealed record LoanWriteOffSummaryResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    string WriteOffNumber,
    string WriteOffType,
    string Reason,
    DateOnly RequestDate,
    DateOnly? WriteOffDate,
    decimal PrincipalWriteOff,
    decimal InterestWriteOff,
    decimal TotalWriteOff,
    decimal RecoveredAmount,
    int DaysPastDue,
    string Status,
    string? ApprovedBy,
    DateTime? ApprovedAt);
