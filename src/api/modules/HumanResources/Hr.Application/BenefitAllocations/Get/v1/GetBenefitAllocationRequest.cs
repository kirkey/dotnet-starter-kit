namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Get.v1;

/// <summary>
/// Request to get benefit allocation details.
/// </summary>
public sealed record GetBenefitAllocationRequest(DefaultIdType Id) : IRequest<BenefitAllocationResponse>;

/// <summary>
/// Response with benefit allocation details.
/// </summary>
public sealed record BenefitAllocationResponse(
    DefaultIdType Id,
    DefaultIdType EnrollmentId,
    string EmployeeName,
    string BenefitName,
    DateTime AllocationDate,
    decimal AllocatedAmount,
    string AllocationType,
    string Status,
    string? ReferenceNumber,
    DateTime? ApprovalDate,
    DateTime? PaymentDate,
    string? Remarks);

