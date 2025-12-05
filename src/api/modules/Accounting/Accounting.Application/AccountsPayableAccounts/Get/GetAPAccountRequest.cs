using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Application.AccountsPayableAccounts.Get;

/// <summary>
/// Request to get an accounts payable account by ID.
/// </summary>
public record GetApAccountRequest(DefaultIdType Id) : IRequest<ApAccountResponse>;

