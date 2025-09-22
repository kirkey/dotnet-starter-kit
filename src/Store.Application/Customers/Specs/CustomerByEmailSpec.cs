namespace FSH.Starter.WebApi.Store.Application.Customers.Specs;

public sealed class CustomerByEmailSpec : Specification<Customer>
{
    public CustomerByEmailSpec(string email) => Query.Where(c => c.Email == email);
}
