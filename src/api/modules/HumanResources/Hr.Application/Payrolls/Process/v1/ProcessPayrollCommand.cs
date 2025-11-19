namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Process.v1;

/// <summary>
/// Command to process a payroll period (initiate pay calculations).
/// Transitions payroll from Draft to Processing status.
/// </summary>
public sealed record ProcessPayrollCommand(
    DefaultIdType Id
) : IRequest<ProcessPayrollResponse>;

/// <summary>
/// Response for payroll processing.
/// </summary>
public sealed record ProcessPayrollResponse(
    DefaultIdType Id,
    string Status,
    DateTime ProcessedDate);

