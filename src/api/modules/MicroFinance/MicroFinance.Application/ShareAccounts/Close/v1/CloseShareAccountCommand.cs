using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Close.v1;

/// <summary>
/// Command to close a share account.
/// </summary>
public sealed record CloseShareAccountCommand(Guid AccountId, string? Reason = null) : IRequest<CloseShareAccountResponse>;
