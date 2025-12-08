using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Reject.v1;

/// <summary>
/// Command to reject a loan restructure request.
/// </summary>
/// <param name="Id">The restructure ID to reject.</param>
/// <param name="UserId">The user rejecting the restructure.</param>
/// <param name="Reason">Reason for rejection.</param>
public sealed record RejectRestructureCommand(
    DefaultIdType Id,
    DefaultIdType UserId,
    string Reason) : IRequest<RejectRestructureResponse>;
