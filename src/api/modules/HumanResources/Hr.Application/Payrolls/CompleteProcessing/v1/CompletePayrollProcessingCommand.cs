namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.CompleteProcessing.v1;

/// <summary>
/// Command to complete payroll processing.
/// Transitions payroll from Processing to Processed status.
/// </summary>
public sealed record CompletePayrollProcessingCommand(
    DefaultIdType Id
) : IRequest<CompletePayrollProcessingResponse>;

/// <summary>
/// Response for completing payroll processing.
/// </summary>
public sealed record CompletePayrollProcessingResponse(
    DefaultIdType Id,
    string Status);

