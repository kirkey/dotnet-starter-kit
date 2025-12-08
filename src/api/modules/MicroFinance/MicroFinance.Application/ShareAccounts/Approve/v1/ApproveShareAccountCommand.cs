using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Approve.v1;

/// <summary>
/// Command to approve a pending share account.
/// </summary>
public sealed record ApproveShareAccountCommand(DefaultIdType ShareAccountId, string? Notes = null) : IRequest<ApproveShareAccountResponse>;
