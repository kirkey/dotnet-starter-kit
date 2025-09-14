namespace Accounting.Application.Payees.Get.v1;
public class PayeeGetRequest(DefaultIdType id) : IRequest<PayeeResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
