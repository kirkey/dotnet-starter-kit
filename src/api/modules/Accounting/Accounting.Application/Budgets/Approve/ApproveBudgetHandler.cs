using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.Budgets.Approve;

/// <summary>
/// Handler for approving a budget.
/// </summary>
public sealed class ApproveBudgetHandler(
    ILogger<ApproveBudgetHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<ApproveBudgetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveBudgetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.BudgetId, cancellationToken);
        
        if (budget == null)
        {
            throw new BudgetNotFoundException(request.BudgetId);
        }

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();

        budget.Approve(approverId, approverName);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Budget {BudgetId} approved by user {ApproverId}", 
            request.BudgetId, approverId);

        return budget.Id;
    }
}
