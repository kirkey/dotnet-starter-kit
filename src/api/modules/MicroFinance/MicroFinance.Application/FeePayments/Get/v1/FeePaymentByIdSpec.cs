using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.FeePayments.Get.v1;

/// <summary>
/// Specification for getting a fee payment by ID.
/// </summary>
public sealed class FeePaymentByIdSpec : Specification<FeePayment, FeePaymentResponse>
{
    public FeePaymentByIdSpec(DefaultIdType id)
    {
        Query.Where(fp => fp.Id == id);

        Query.Select(fp => new FeePaymentResponse(
            fp.Id,
            fp.FeeChargeId,
            fp.LoanRepaymentId,
            fp.SavingsTransactionId,
            fp.Reference,
            fp.PaymentDate,
            fp.Amount,
            fp.PaymentMethod,
            fp.PaymentSource,
            fp.Status,
            fp.ReversalReason,
            fp.ReversedDate,
            fp.Notes));
    }
}
