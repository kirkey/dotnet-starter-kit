using Accounting.Domain;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Budgets.Delete;

public sealed class DeleteBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IRepository<Budget> repository)
    : IRequestHandler<DeleteBudgetRequest>
{
    public async Task Handle(DeleteBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var budget = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (budget == null) throw new BudgetNotFoundException(request.Id);

        await repository.DeleteAsync(budget, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
