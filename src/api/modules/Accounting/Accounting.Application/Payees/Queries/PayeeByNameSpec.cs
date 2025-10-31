namespace Accounting.Application.Payees.Queries;

public class PayeeByNameSpec : Specification<Payee>
{
    public PayeeByNameSpec(string name)
    {
        Query.Where(p => p.Name == name);
    }
}
