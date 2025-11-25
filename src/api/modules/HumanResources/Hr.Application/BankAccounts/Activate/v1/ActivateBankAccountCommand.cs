namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Activate.v1;

/// <summary>
/// Command to activate a deactivated bank account.
/// </summary>
public sealed record ActivateBankAccountCommand(
    DefaultIdType Id
) : IRequest<ActivateBankAccountResponse>;

/// <summary>
/// Response for bank account activation.
/// </summary>
public sealed record ActivateBankAccountResponse(
    DefaultIdType Id,
    bool IsActive,
    string BankName);

