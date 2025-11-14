namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Create.v1;

/// <summary>
/// Response for creating a bank account.
/// </summary>
/// <param name="Id">The identifier of the created bank account.</param>
public sealed record CreateBankAccountResponse(DefaultIdType Id);

