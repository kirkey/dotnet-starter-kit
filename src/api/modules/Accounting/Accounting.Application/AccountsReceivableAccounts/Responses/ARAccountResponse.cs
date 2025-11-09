namespace Accounting.Application.AccountsReceivableAccounts.Responses;

/// <summary>
/// Response containing accounts receivable account details.
/// </summary>
public record ArAccountResponse
{
    public DefaultIdType Id { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public DefaultIdType? CustomerId { get; init; }
    public decimal Balance { get; init; }
    public bool IsActive { get; init; }
}
