namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Deactivate.v1;

/// <summary>
/// Command to deactivate a bank account.
/// </summary>
public sealed record DeactivateBankAccountCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Reason = null
) : IRequest<DeactivateBankAccountResponse>;

/// <summary>
/// Response for bank account deactivation.
/// </summary>
public sealed record DeactivateBankAccountResponse(
    DefaultIdType Id,
    bool IsActive,
    string BankName);

