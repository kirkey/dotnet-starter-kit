

namespace FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

public class GetCustomerSpecs : Specification<Customer, CustomerResponse>
{
    public GetCustomerSpecs(DefaultIdType id)
    {
        Query
            .Where(c => c.Id == id);
    }
}
