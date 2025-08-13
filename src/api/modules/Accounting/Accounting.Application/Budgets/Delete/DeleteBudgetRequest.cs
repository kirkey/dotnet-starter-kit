using MediatR;

namespace Accounting.Application.Budgets.Delete;

public class DeleteBudgetRequest : IRequest
{
    public DefaultIdType Id { get; set; }

    public DeleteBudgetRequest(DefaultIdType id)
    {
        Id = id;
    }
}
