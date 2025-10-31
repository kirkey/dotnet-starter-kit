using Accounting.Application.PrepaidExpenses.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.PrepaidExpenses.Create.v1;

/// <summary>
/// Handler for creating a new prepaid expense.
/// </summary>
public sealed class PrepaidExpenseCreateHandler(
    ILogger<PrepaidExpenseCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<PrepaidExpense> repository)
    : IRequestHandler<PrepaidExpenseCreateCommand, PrepaidExpenseCreateResponse>
{
    public async Task<PrepaidExpenseCreateResponse> Handle(PrepaidExpenseCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate prepaid number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new PrepaidExpenseByNumberSpec(request.PrepaidNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicatePrepaidExpenseNumberException(request.PrepaidNumber);
        }

        var prepaidExpense = PrepaidExpense.Create(
            prepaidNumber: request.PrepaidNumber,
            description: request.Description,
            totalAmount: request.TotalAmount,
            startDate: request.StartDate,
            endDate: request.EndDate,
            prepaidAssetAccountId: request.PrepaidAssetAccountId,
            expenseAccountId: request.ExpenseAccountId,
            paymentDate: request.PaymentDate,
            amortizationSchedule: request.AmortizationSchedule,
            vendorId: request.VendorId,
            vendorName: request.VendorName,
            paymentId: request.PaymentId,
            costCenterId: request.CostCenterId,
            periodId: request.PeriodId,
            notes: request.Notes);

        await repository.AddAsync(prepaidExpense, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Prepaid expense created {PrepaidExpenseId} - {PrepaidNumber}", 
            prepaidExpense.Id, prepaidExpense.PrepaidNumber);
        return new PrepaidExpenseCreateResponse(prepaidExpense.Id);
    }
}

