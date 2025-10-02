using Accounting.Application.BankReconciliations.Responses;

namespace Accounting.Application.BankReconciliations.Get.v1;

public class GetBankReconciliationRequest(DefaultIdType id) : IRequest<BankReconciliationResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
