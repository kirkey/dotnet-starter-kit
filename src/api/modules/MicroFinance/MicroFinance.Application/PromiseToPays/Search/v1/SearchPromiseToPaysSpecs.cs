using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Search.v1;

public class SearchPromiseToPaysSpecs : EntitiesByPaginationFilterSpec<PromiseToPay, PromiseToPaySummaryResponse>
{
    public SearchPromiseToPaysSpecs(SearchPromiseToPaysCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.PromisedPaymentDate, !command.HasOrderBy())
            .Where(x => x.CollectionCaseId == command.CollectionCaseId!.Value, command.CollectionCaseId.HasValue)
            .Where(x => x.LoanId == command.LoanId!.Value, command.LoanId.HasValue)
            .Where(x => x.MemberId == command.MemberId!.Value, command.MemberId.HasValue)
            .Where(x => x.CollectionActionId == command.CollectionActionId!.Value, command.CollectionActionId.HasValue)
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.PromiseDate >= command.PromiseDateFrom!.Value, command.PromiseDateFrom.HasValue)
            .Where(x => x.PromiseDate <= command.PromiseDateTo!.Value, command.PromiseDateTo.HasValue)
            .Where(x => x.PromisedPaymentDate >= command.PromisedPaymentDateFrom!.Value, command.PromisedPaymentDateFrom.HasValue)
            .Where(x => x.PromisedPaymentDate <= command.PromisedPaymentDateTo!.Value, command.PromisedPaymentDateTo.HasValue)
            .Where(x => x.PromisedAmount >= command.MinPromisedAmount!.Value, command.MinPromisedAmount.HasValue)
            .Where(x => x.PromisedAmount <= command.MaxPromisedAmount!.Value, command.MaxPromisedAmount.HasValue);
}
