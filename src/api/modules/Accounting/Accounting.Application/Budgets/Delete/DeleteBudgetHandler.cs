namespace Accounting.Application.Budgets.Delete;

using Exceptions;

public sealed class DeleteBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<DeleteBudgetRequest>
{
    public async Task Handle(DeleteBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (budget == null) throw new BudgetNotFoundException(request.Id);

        // Prevent deletion of approved or active budgets
        if (budget.Status == "Approved" || budget.Status == "Active")
            throw new BudgetCannotBeDeletedException(request.Id);

        await repository.DeleteAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
