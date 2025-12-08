// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/modules/MicroFinance/MicroFinance.Application/UssdSessions/Search/v1/SearchUssdSessionsCommand.cs
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.UssdSessions.Search.v1;

/// <summary>
/// Command for searching USSD sessions with pagination and filters.
/// </summary>
public class SearchUssdSessionsCommand : PaginationFilter, IRequest<PagedList<UssdSessionResponse>>
{
    /// <summary>
    /// Filter by phone number.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Filter by member ID.
    /// </summary>
    public DefaultIdType? MemberId { get; set; }

    /// <summary>
    /// Filter by status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by session start date from.
    /// </summary>
    public DateOnly? StartedFrom { get; set; }

    /// <summary>
    /// Filter by session start date to.
    /// </summary>
    public DateOnly? StartedTo { get; set; }

    /// <summary>
    /// Filter by current operation.
    /// </summary>
    public string? CurrentOperation { get; set; }
}

