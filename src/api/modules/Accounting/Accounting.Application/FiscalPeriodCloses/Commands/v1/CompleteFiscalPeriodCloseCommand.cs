namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to complete the fiscal period close process.
/// The user who completes the close is automatically determined from the current user session.
/// </summary>
public record CompleteFiscalPeriodCloseCommand(
    DefaultIdType FiscalPeriodCloseId
) : IRequest<DefaultIdType>;

