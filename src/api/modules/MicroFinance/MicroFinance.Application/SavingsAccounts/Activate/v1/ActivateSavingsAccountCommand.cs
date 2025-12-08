using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Activate.v1;

/// <summary>
/// Command to activate a pending savings account.
/// </summary>
public sealed record ActivateSavingsAccountCommand(DefaultIdType SavingsAccountId) : IRequest<ActivateSavingsAccountResponse>;
