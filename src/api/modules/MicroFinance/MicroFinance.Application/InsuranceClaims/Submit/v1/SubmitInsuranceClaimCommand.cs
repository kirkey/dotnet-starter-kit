using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

/// <summary>
/// Command to submit a new insurance claim.
/// </summary>
public sealed record SubmitInsuranceClaimCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType PolicyId,
    [property: DefaultValue("Death")] string ClaimType,
    [property: DefaultValue(500000)] decimal ClaimAmount,
    [property: DefaultValue("2025-01-10")] DateOnly IncidentDate,
    [property: DefaultValue("Claim due to covered event")] string Description,
    [property: DefaultValue(null)] string? SupportingDocuments = null
) : IRequest<SubmitInsuranceClaimResponse>;
