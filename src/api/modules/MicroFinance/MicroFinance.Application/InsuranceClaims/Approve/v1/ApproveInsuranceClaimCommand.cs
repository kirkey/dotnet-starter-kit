using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Approve.v1;

/// <summary>
/// Command to approve an insurance claim.
/// </summary>
public sealed record ApproveInsuranceClaimCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue(450000)] decimal ApprovedAmount,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid ApproverId,
    [property: DefaultValue("Claim approved per policy terms")] string? ApprovalNotes = null
) : IRequest<ApproveInsuranceClaimResponse>;
