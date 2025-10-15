namespace Accounting.Application.Checks.Create.v1;

/// <summary>
/// Command to register a new check in the system.
/// </summary>
public record CheckCreateCommand(
    string CheckNumber,
    string BankAccountCode,
    string? BankAccountName,
    string? Description,
    string? Notes
) : IRequest<CheckCreateResponse>;

