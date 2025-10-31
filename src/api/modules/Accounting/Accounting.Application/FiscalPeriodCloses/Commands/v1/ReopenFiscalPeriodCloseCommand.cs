namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to reopen a completed fiscal period close.
/// </summary>
public record ReopenFiscalPeriodCloseCommand(
    DefaultIdType FiscalPeriodCloseId,
    string ReopenedBy,
    string Reason
) : IRequest<DefaultIdType>;

