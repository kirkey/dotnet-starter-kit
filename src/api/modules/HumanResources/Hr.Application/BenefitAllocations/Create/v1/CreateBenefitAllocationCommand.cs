namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Create.v1;

/// <summary>
/// Command to create a new benefit allocation.
/// </summary>
public sealed record CreateBenefitAllocationCommand(
    DefaultIdType EnrollmentId,
    [property: DefaultValue(null)] DateTime? AllocationDate = null,
    [property: DefaultValue(0)] decimal AllocatedAmount = 0,
    [property: DefaultValue("Usage")] string AllocationType = "Usage",
    [property: DefaultValue(null)] string? ReferenceNumber = null,
    [property: DefaultValue(null)] string? Remarks = null
) : IRequest<CreateBenefitAllocationResponse>;

/// <summary>
/// Response for benefit allocation creation.
/// </summary>
public sealed record CreateBenefitAllocationResponse(
    DefaultIdType Id,
    DefaultIdType EnrollmentId,
    DateTime AllocationDate,
    decimal AllocatedAmount,
    string AllocationType,
    string Status);

