using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Search.v1;

public class SearchCommunicationLogsSpecs : EntitiesByPaginationFilterSpec<CommunicationLog, CommunicationLogSummaryResponse>
{
    public SearchCommunicationLogsSpecs(SearchCommunicationLogsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.CreatedOn, !command.HasOrderBy())
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.TemplateId == command.TemplateId!.Value, command.TemplateId.HasValue)
            .Where(x => x.Channel == command.Channel, !string.IsNullOrWhiteSpace(command.Channel))
            .Where(x => x.DeliveryStatus == command.DeliveryStatus, !string.IsNullOrWhiteSpace(command.DeliveryStatus))
            .Where(x => x.Recipient.Contains(command.Recipient!), !string.IsNullOrWhiteSpace(command.Recipient))
            .Where(x => x.SentAt >= command.SentFrom!.Value, command.SentFrom.HasValue)
            .Where(x => x.SentAt <= command.SentTo!.Value, command.SentTo.HasValue);
}
