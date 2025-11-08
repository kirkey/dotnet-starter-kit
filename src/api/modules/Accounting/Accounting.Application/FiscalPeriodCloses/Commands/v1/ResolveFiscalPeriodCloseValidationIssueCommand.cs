namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to resolve a validation issue in the fiscal period close process.
/// </summary>
public sealed record ResolveFiscalPeriodCloseValidationIssueCommand(
    DefaultIdType FiscalPeriodCloseId,
    string IssueDescription,
    string Resolution
) : IRequest<DefaultIdType>;

