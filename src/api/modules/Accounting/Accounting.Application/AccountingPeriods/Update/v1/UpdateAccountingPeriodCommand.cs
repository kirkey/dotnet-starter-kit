namespace Accounting.Application.AccountingPeriods.Update.v1;

/// <summary>
/// Command to update an existing accounting period.
/// Only mutable fields are included; domain invariants are validated by the handler.
/// </summary>
/// <param name="Id">Identifier of the accounting period to update.</param>
/// <param name="Name">Optional new name for the period.</param>
/// <param name="StartDate">Optional new start date.</param>
/// <param name="EndDate">Optional new end date.</param>
/// <param name="IsAdjustmentPeriod">Flag indicating adjustment period.</param>
/// <param name="FiscalYear">Optional fiscal year.</param>
/// <param name="PeriodType">Optional period type (Monthly, Quarterly, Yearly, Annual).</param>
/// <param name="Description">Optional long description.</param>
/// <param name="Notes">Optional administrative notes.</param>
public record UpdateAccountingPeriodCommand(
    DefaultIdType Id,
    string? Name = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    bool IsAdjustmentPeriod = false,
    int? FiscalYear = null,
    string? PeriodType = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
