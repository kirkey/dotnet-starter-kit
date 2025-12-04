using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;

/// <summary>
/// Command to close a savings account.
/// </summary>
public sealed record CloseAccountCommand(Guid AccountId, string? Reason = null) : IRequest<CloseAccountResponse>;
