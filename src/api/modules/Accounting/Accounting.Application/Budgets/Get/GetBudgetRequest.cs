using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Get;

public class GetBudgetRequest(DefaultIdType id) : IRequest<BudgetDto>
{
    public DefaultIdType Id { get; set; } = id;
}
