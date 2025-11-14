namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;

/// <summary>
/// Response for deleting a bank account.
/// </summary>
/// <param name="Id">The identifier of the deleted bank account.</param>
public sealed record DeleteBankAccountResponse(DefaultIdType Id);

