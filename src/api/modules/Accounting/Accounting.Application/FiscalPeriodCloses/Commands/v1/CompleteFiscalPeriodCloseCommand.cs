namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to complete the fiscal period close process.
/// </summary>
public record CompleteFiscalPeriodCloseCommand(
    DefaultIdType FiscalPeriodCloseId,
    string CompletedBy
) : IRequest<DefaultIdType>;

