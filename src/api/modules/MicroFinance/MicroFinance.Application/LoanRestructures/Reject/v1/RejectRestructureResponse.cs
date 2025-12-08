namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Reject.v1;

/// <summary>
/// Response after rejecting a loan restructure.
/// </summary>
/// <param name="Id">The restructure ID.</param>
/// <param name="Status">The new status.</param>
/// <param name="Reason">The rejection reason.</param>
public sealed record RejectRestructureResponse(
    DefaultIdType Id,
    string Status,
    string Reason);
