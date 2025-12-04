using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Reject.v1;

/// <summary>
/// Command to reject an insurance claim.
/// </summary>
public sealed record RejectInsuranceClaimCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue("Claim does not meet policy requirements")] string Reason,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid RejectedById
) : IRequest<RejectInsuranceClaimResponse>;
