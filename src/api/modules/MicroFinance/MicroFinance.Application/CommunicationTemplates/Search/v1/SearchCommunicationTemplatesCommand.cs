using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Search.v1;

public class SearchCommunicationTemplatesCommand : PaginationFilter, IRequest<PagedList<CommunicationTemplateSummaryResponse>>
{
    public string? Code { get; set; }
    public string? Channel { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; }
    public string? Status { get; set; }
    public bool? RequiresApproval { get; set; }
}

public sealed record CommunicationTemplateSummaryResponse(
    DefaultIdType Id,
    string Code,
    string Channel,
    string Category,
    string? Subject,
    string Language,
    bool RequiresApproval,
    string Status
);
