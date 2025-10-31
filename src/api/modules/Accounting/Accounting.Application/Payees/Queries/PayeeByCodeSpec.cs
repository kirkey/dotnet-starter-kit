namespace Accounting.Application.Payees.Queries;

public class PayeeByCodeSpec : Specification<Payee>
{
    public PayeeByCodeSpec(string code)
    {
        Query.Where(p => p.PayeeCode == code);
    }
}
