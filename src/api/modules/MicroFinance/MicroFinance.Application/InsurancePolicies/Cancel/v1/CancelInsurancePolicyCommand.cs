using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Cancel.v1;

/// <summary>
/// Command to cancel an insurance policy.
/// </summary>
public sealed record CancelInsurancePolicyCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("Policy cancelled by member request")] string Reason
) : IRequest<CancelInsurancePolicyResponse>;
