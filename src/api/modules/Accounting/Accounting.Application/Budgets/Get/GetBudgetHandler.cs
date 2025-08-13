using Accounting.Domain;
using Accounting.Application.Budgets.Dtos;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Budgets.Get;

public sealed class GetBudgetHandler(
    [FromKeyedServices("accounting:budgets")] IReadRepository<Budget> repository,
    ICacheService cache)
    : IRequestHandler<GetBudgetRequest, BudgetDto>
{
    public async Task<BudgetDto> Handle(GetBudgetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"budget:{request.Id}",
            async () =>
            {
                var budget = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (budget == null) throw new BudgetNotFoundException(request.Id);
                return new BudgetDto(
                    budget.Id,
                    budget.Name!,
                    budget.PeriodId,
                    budget.FiscalYear,
                    budget.BudgetType,
                    budget.Status,
                    budget.TotalBudgetedAmount,
                    budget.TotalActualAmount,
                    budget.ApprovedDate,
                    budget.ApprovedBy,
                    budget.Description,
                    budget.Notes);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
