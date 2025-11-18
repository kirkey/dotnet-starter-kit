namespace Accounting.Application.AccountsPayableAccounts.Responses;

public record ApAccountResponse
{
    public DefaultIdType Id { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public string AccountType { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public decimal CurrentBalance { get; init; }
    public string? Description { get; init; }
}

