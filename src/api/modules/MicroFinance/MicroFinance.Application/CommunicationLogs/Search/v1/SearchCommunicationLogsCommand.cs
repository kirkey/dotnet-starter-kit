using FSH.Framework.Core.Paging;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Search.v1;

public class SearchCommunicationLogsCommand : PaginationFilter, IRequest<PagedList<CommunicationLogSummaryResponse>>
{
    public DefaultIdType? MemberId { get; set; }
    public DefaultIdType? LoanId { get; set; }
    public DefaultIdType? TemplateId { get; set; }
    public string? Channel { get; set; }
    public string? DeliveryStatus { get; set; }
    public string? Recipient { get; set; }
    public DateTime? SentFrom { get; set; }
    public DateTime? SentTo { get; set; }
}

public sealed record CommunicationLogSummaryResponse(
    DefaultIdType Id,
    DefaultIdType? TemplateId,
    DefaultIdType? MemberId,
    DefaultIdType? LoanId,
    string Channel,
    string Recipient,
    string? Subject,
    string DeliveryStatus,
    DateTime? SentAt,
    DateTime? DeliveredAt,
    int RetryCount,
    string? ErrorMessage
);
