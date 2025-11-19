namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;

/// <summary>
/// Command to delete bank account.
/// </summary>
public sealed record DeleteBankAccountCommand(
    DefaultIdType Id
) : IRequest<DeleteBankAccountResponse>;

/// <summary>
/// Response for bank account deletion.
/// </summary>
public sealed record DeleteBankAccountResponse(
    DefaultIdType Id,
    bool Success);

