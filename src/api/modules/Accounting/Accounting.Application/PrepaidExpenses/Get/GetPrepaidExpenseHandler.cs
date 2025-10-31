using Accounting.Application.PrepaidExpenses.Queries;
using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Application.PrepaidExpenses.Get;

/// <summary>
/// Handler for retrieving a prepaid expense by ID.
/// </summary>
public class GetPrepaidExpenseHandler(
    [FromKeyedServices("accounting")] IReadRepository<PrepaidExpense> repository)
    : IRequestHandler<GetPrepaidExpenseRequest, PrepaidExpenseResponse>
{
    public async Task<PrepaidExpenseResponse> Handle(
        GetPrepaidExpenseRequest request,
        CancellationToken cancellationToken)
    {
        var expense = await repository.FirstOrDefaultAsync(
            new PrepaidExpenseByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (expense is null)
        {
            throw new NotFoundException(
                $"{nameof(PrepaidExpense)} with ID {request.Id} was not found.");
        }

        return new PrepaidExpenseResponse
        {
            Id = expense.Id,
            PrepaidNumber = expense.PrepaidNumber,
            TotalAmount = expense.TotalAmount,
            AmortizedAmount = expense.AmortizedAmount,
            RemainingAmount = expense.RemainingAmount,
            StartDate = expense.StartDate,
            EndDate = expense.EndDate,
            AmortizationSchedule = expense.AmortizationSchedule,
            Status = expense.Status,
            PrepaidAssetAccountId = expense.PrepaidAssetAccountId,
            ExpenseAccountId = expense.ExpenseAccountId,
            VendorId = expense.VendorId,
            VendorName = expense.VendorName,
            IsFullyAmortized = expense.IsFullyAmortized,
            Description = expense.Description,
            Notes = expense.Notes
        };
    }
}

