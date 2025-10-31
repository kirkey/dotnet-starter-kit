using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Application.AccountsReceivableAccounts.Search.v1;

/// <summary>
/// Request to search for accounts receivable accounts with optional filters.
/// </summary>
public record SearchARAccountsRequest(string? AccountNumber = null) : IRequest<List<ARAccountResponse>>;

