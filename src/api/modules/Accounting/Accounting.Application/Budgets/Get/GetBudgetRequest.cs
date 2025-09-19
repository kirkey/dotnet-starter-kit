using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Get;

public class GetBudgetQuery(DefaultIdType id) : IRequest<BudgetDto>
{
    public DefaultIdType Id { get; set; } = id;
}
