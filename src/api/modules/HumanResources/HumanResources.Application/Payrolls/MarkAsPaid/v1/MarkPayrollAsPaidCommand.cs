namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.MarkAsPaid.v1;

/// <summary>
/// Command to mark a payroll as paid.
/// Transitions payroll from Posted to Paid status.
/// </summary>
public sealed record MarkPayrollAsPaidCommand(
    DefaultIdType Id
) : IRequest<MarkPayrollAsPaidResponse>;

/// <summary>
/// Response for marking payroll as paid.
/// </summary>
public sealed record MarkPayrollAsPaidResponse(
    DefaultIdType Id,
    string Status,
    DateTime PaidDate);

