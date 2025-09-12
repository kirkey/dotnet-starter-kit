namespace FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

public class GetCustomerRequest(DefaultIdType id) : IRequest<CustomerResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
