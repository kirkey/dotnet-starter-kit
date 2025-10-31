namespace Accounting.Application.FiscalPeriodCloses.Commands.v1;

/// <summary>
/// Command to complete a task in the fiscal period close process.
/// </summary>
public record CompleteTaskCommand(
    DefaultIdType FiscalPeriodCloseId,
    string TaskName
) : IRequest<DefaultIdType>;

