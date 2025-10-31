using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Search.v1;

/// <summary>
/// Request to search for accounts payable accounts with optional filters.
/// </summary>
public record SearchAPAccountsRequest(string? AccountNumber = null) : IRequest<List<APAccountResponse>>;

