namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

/// <summary>
/// Request to get bank account details.
/// </summary>
public sealed record GetBankAccountRequest(DefaultIdType Id) : IRequest<BankAccountResponse>;

