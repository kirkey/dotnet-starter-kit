using MediatR;
using Accounting.Application.Budgets.Dtos;

namespace Accounting.Application.Budgets.Get;

public class GetBudgetRequest : IRequest<BudgetDto>
{
    public DefaultIdType Id { get; set; }

    public GetBudgetRequest(DefaultIdType id)
    {
        Id = id;
    }
}
