namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Delete.v1;

/// <summary>
/// Command to delete a bank account.
/// </summary>
public sealed record DeleteBankAccountCommand(DefaultIdType Id) : IRequest<DeleteBankAccountResponse>;

