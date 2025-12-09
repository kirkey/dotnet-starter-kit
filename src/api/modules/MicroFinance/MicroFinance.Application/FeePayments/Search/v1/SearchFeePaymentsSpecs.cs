using Ardalis.Specification;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Search.v1;

public class SearchFeePaymentsSpecs : EntitiesByPaginationFilterSpec<FeePayment, FeePaymentSummaryResponse>
{
    public SearchFeePaymentsSpecs(SearchFeePaymentsCommand command)
        : base(command) =>
        Query
            .OrderByDescending(x => x.PaymentDate, !command.HasOrderBy())
            .ThenByDescending(x => x.CreatedOn)
            .Where(x => x.FeeChargeId == command.FeeChargeId!.Value, command.FeeChargeId.HasValue)
            .Where(x => x.Reference.Contains(command.Reference!), !string.IsNullOrWhiteSpace(command.Reference))
            .Where(x => x.PaymentMethod == command.PaymentMethod, !string.IsNullOrWhiteSpace(command.PaymentMethod))
            .Where(x => x.PaymentSource == command.PaymentSource, !string.IsNullOrWhiteSpace(command.PaymentSource))
            .Where(x => x.Status == command.Status, !string.IsNullOrWhiteSpace(command.Status))
            .Where(x => x.PaymentDate >= command.PaymentDateFrom!.Value, command.PaymentDateFrom.HasValue)
            .Where(x => x.PaymentDate <= command.PaymentDateTo!.Value, command.PaymentDateTo.HasValue);
}
