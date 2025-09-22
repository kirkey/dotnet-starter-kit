namespace FSH.Starter.WebApi.Store.Application.Customers.Specs;

public sealed class CustomerByCodeSpec : Specification<Customer>
{
    public CustomerByCodeSpec(string code) => Query.Where(c => c.Code == code);
}
