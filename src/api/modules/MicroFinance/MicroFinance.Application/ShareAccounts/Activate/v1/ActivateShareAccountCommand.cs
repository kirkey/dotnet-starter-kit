using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Activate.v1;

/// <summary>
/// Command to activate an approved share account.
/// </summary>
public sealed record ActivateShareAccountCommand(DefaultIdType ShareAccountId) : IRequest<ActivateShareAccountResponse>;
