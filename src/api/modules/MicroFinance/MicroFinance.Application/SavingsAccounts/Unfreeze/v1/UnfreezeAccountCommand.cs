using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Unfreeze.v1;

/// <summary>
/// Command to unfreeze a savings account.
/// </summary>
public sealed record UnfreezeAccountCommand(DefaultIdType AccountId) : IRequest<UnfreezeAccountResponse>;
