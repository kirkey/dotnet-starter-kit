using Accounting.Domain;
using Ardalis.Specification;

namespace Accounting.Application.Payees.Get.v1;

public class PayeeGetSpecs : Specification<Payee, PayeeResponse>
{
    public PayeeGetSpecs(DefaultIdType id)
    {
        Query
            .Where(p => p.Id == id);
    }
}
