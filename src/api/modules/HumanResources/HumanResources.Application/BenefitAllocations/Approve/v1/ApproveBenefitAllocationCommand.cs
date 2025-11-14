namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Approve.v1;

/// <summary>
/// Command to approve benefit allocation.
/// </summary>
public sealed record ApproveBenefitAllocationCommand(
    DefaultIdType Id,
    DefaultIdType ApprovedBy
) : IRequest<ApproveBenefitAllocationResponse>;

/// <summary>
/// Response for benefit allocation approval.
/// </summary>
public sealed record ApproveBenefitAllocationResponse(
    DefaultIdType Id,
    string Status,
    DateTime ApprovalDate,
    DefaultIdType ApprovedBy);

