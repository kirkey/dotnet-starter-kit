namespace Accounting.Application.PrepaidExpenses.Create.v1;

/// <summary>
/// Command to create a new prepaid expense.
/// </summary>
public record PrepaidExpenseCreateCommand(
    string PrepaidNumber,
    string Description,
    decimal TotalAmount,
    DateTime StartDate,
    DateTime EndDate,
    DefaultIdType PrepaidAssetAccountId,
    DefaultIdType ExpenseAccountId,
    DateTime PaymentDate,
    string AmortizationSchedule = "Monthly",
    DefaultIdType? VendorId = null,
    string? VendorName = null,
    DefaultIdType? PaymentId = null,
    DefaultIdType? CostCenterId = null,
    DefaultIdType? PeriodId = null,
    string? Notes = null
) : IRequest<PrepaidExpenseCreateResponse>;

