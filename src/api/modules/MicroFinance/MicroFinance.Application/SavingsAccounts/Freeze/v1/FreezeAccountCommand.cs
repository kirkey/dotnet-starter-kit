using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Command to freeze a savings account.
/// </summary>
public sealed record FreezeAccountCommand(Guid AccountId, string? Reason = null) : IRequest<FreezeAccountResponse>;
