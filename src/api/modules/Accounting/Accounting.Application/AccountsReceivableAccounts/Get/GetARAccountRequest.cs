using Accounting.Application.AccountsReceivableAccounts.Responses;

namespace Accounting.Application.AccountsReceivableAccounts.Get;

/// <summary>
/// Request to get an accounts receivable account by ID.
/// </summary>
public record GetARAccountRequest(DefaultIdType Id) : IRequest<ARAccountResponse>;

