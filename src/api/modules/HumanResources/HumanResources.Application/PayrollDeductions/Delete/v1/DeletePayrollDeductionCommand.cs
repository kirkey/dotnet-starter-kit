namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Delete.v1;

/// <summary>
/// Command to delete payroll deduction.
/// </summary>
public sealed record DeletePayrollDeductionCommand(
    DefaultIdType Id
) : IRequest<DeletePayrollDeductionResponse>;

/// <summary>
/// Response for deduction deletion.
/// </summary>
public sealed record DeletePayrollDeductionResponse(
    DefaultIdType Id,
    bool Success);

