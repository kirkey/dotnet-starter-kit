using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.QrPayments.Get.v1;

public sealed class QrPaymentByIdSpec : Specification<QrPayment>
{
    public QrPaymentByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}
