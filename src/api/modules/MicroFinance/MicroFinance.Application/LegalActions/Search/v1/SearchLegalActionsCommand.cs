// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/LegalActions/Search/v1/SearchLegalActionsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LegalActions.Search.v1;

/// <summary>
/// Command for searching legal actions with pagination and filters.
/// </summary>
public class SearchLegalActionsCommand : PaginationFilter, IRequest<PagedList<LegalActionResponse>>
{
    /// <summary>
    /// Filter by loan ID.
    /// </summary>
    public DefaultIdType? LoanId { get; set; }

    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by collection case ID.
    /// </summary>
    public DefaultIdType? CollectionCaseId { get; set; }

    /// <summary>
    /// Filter by action type.
    /// </summary>
    public string? ActionType { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by court name.
    /// </summary>
    public string? CourtName { get; set; }

    /// <summary>
    /// Filter by initiated date from.
    /// </summary>
    public DateOnly? InitiatedDateFrom { get; set; }

    /// <summary>
    /// Filter by initiated date to.
    /// </summary>
    public DateOnly? InitiatedDateTo { get; set; }
}

