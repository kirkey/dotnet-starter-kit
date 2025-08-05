using Accounting.Domain;
using Accounting.Application.Budgets.Exceptions;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Budgets.Update;

public sealed class UpdateBudgetHandler(
    [FromKeyedServices("accounting")] IRepository<Budget> repository)
    : IRequestHandler<UpdateBudgetRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (budget == null) throw new BudgetNotFoundException(request.Id);

        budget.Update(request.Name, request.BudgetType, request.Status, request.Description, request.Notes);

        await repository.UpdateAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}
