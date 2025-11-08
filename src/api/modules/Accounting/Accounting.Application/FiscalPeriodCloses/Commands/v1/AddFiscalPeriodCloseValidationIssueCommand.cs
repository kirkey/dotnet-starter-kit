namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to add a validation issue to the fiscal period close process.
/// </summary>
public sealed record AddFiscalPeriodCloseValidationIssueCommand(
    DefaultIdType FiscalPeriodCloseId,
    string IssueDescription,
    string Severity,
    string? Resolution = null
) : IRequest<DefaultIdType>;
