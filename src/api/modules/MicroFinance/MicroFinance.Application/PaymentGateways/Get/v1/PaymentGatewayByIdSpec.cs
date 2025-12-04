using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Get.v1;

public sealed class PaymentGatewayByIdSpec : Specification<PaymentGateway>
{
    public PaymentGatewayByIdSpec(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
