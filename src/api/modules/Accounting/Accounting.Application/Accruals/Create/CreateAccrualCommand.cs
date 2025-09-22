namespace Accounting.Application.Accruals.Create;

public sealed record CreateAccrualCommand(
    string AccrualNumber,
    DateTime AccrualDate,
    decimal Amount,
    string? Description
) : IRequest<CreateAccrualResponse>;

