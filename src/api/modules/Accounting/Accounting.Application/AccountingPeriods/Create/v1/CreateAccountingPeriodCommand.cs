namespace Accounting.Application.AccountingPeriods.Create.v1;

/// <summary>
/// Command to create a new accounting period.
/// </summary>
/// <param name="Name">Human-friendly name for the period (required, max length enforced by domain).</param>
/// <param name="StartDate">Inclusive start date of the period.</param>
/// <param name="EndDate">Inclusive end date of the period; must be after <paramref name="StartDate"/>.</param>
/// <param name="FiscalYear">Fiscal year the period belongs to (e.g. 2025).</param>
/// <param name="PeriodType">Period granularity, e.g. "Monthly", "Quarterly", "Yearly".</param>
/// <param name="IsAdjustmentPeriod">True when this period is an adjustment period (e.g. period 13).</param>
/// <param name="Description">Optional long description.</param>
/// <param name="Notes">Optional admin notes.</param>
public sealed record CreateAccountingPeriodCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int FiscalYear,
    string PeriodType,
    bool IsAdjustmentPeriod = false,
    string? Description = null,
    string? Notes = null
) : IRequest<DefaultIdType>;
