using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Get.v1;

/// <summary>
/// Specification for getting a promise to pay by ID.
/// </summary>
public sealed class PromiseToPayByIdSpec : Specification<PromiseToPay, PromiseToPayResponse>
{
    public PromiseToPayByIdSpec(Guid id)
    {
        Query.Where(p => p.Id == id);

        Query.Select(p => new PromiseToPayResponse(
            p.Id,
            p.CollectionCaseId,
            p.LoanId,
            p.MemberId,
            p.PromiseDate,
            p.PromisedPaymentDate,
            p.PromisedAmount,
            p.ActualAmountPaid,
            p.ActualPaymentDate,
            p.Status,
            p.RescheduleCount,
            p.BreachReason));
    }
}
