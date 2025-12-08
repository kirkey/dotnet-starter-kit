using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Command to freeze a savings account.
/// </summary>
public sealed record FreezeAccountCommand(DefaultIdType AccountId, string? Reason = null) : IRequest<FreezeAccountResponse>;
