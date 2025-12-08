using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Reject.v1;

/// <summary>
/// Command to reject a loan guarantor.
/// </summary>
public sealed record RejectGuarantorCommand(DefaultIdType Id, string? Reason = null) : IRequest<RejectGuarantorResponse>;
