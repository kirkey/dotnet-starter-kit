using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Settle.v1;

/// <summary>
/// Command to settle an approved insurance claim.
/// </summary>
public sealed record SettleInsuranceClaimCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue("2025-01-20")] DateOnly SettlementDate,
    [property: DefaultValue("CHK-001")] string? PaymentReference = null
) : IRequest<SettleInsuranceClaimResponse>;
