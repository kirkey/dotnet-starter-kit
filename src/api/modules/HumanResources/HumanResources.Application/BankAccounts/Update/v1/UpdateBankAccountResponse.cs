namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Update.v1;

/// <summary>
/// Response for updating a bank account.
/// </summary>
/// <param name="Id">The identifier of the updated bank account.</param>
public sealed record UpdateBankAccountResponse(DefaultIdType Id);

