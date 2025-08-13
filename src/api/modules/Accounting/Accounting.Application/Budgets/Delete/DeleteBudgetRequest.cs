using MediatR;

namespace Accounting.Application.Budgets.Delete;

public class DeleteBudgetRequest(DefaultIdType id) : IRequest
{
    public DefaultIdType Id { get; set; } = id;
}
