namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

/// <summary>
/// Request to get a bank account by its identifier.
/// </summary>
public sealed record GetBankAccountRequest(DefaultIdType Id) : IRequest<BankAccountResponse>;

