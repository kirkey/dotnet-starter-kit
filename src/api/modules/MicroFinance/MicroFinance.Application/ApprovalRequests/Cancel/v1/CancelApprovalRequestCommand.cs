using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ApprovalRequests.Cancel.v1;

/// <summary>
/// Command to cancel an approval request.
/// </summary>
public sealed record CancelApprovalRequestCommand(Guid Id, string Reason) : IRequest<CancelApprovalRequestResponse>;
