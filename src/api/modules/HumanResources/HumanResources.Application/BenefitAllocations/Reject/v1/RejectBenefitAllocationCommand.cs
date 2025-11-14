namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Reject.v1;

/// <summary>
/// Command to reject benefit allocation.
/// </summary>
public sealed record RejectBenefitAllocationCommand(
    DefaultIdType Id,
    DefaultIdType RejectedBy,
    [property: DefaultValue(null)] string? Reason = null
) : IRequest<RejectBenefitAllocationResponse>;

/// <summary>
/// Response for benefit allocation rejection.
/// </summary>
public sealed record RejectBenefitAllocationResponse(
    DefaultIdType Id,
    string Status,
    string? Reason);

