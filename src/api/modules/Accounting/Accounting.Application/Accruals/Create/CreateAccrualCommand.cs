namespace Accounting.Application.Accruals.Create;

/// <summary>
/// Command to create a new Accrual.
/// </summary>
/// <param name="AccrualNumber">Unique human-readable number (max 50).</param>
/// <param name="AccrualDate">Accounting date of the accrual.</param>
/// <param name="Amount">Positive decimal amount.</param>
/// <param name="Description">Optional description (max 200).</param>
public sealed record CreateAccrualCommand(
    string AccrualNumber,
    DateTime AccrualDate,
    decimal Amount,
    string? Description = null
) : IRequest<CreateAccrualResponse>;
