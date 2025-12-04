using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Review.v1;

/// <summary>
/// Command to review an insurance claim.
/// </summary>
public sealed record ReviewInsuranceClaimCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid ReviewerId,
    [property: DefaultValue("Claim documents verified")] string ReviewNotes
) : IRequest<ReviewInsuranceClaimResponse>;
