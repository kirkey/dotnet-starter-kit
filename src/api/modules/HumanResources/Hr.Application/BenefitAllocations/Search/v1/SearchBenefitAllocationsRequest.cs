namespace FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Search.v1;

/// <summary>
/// Request to search benefit allocations.
/// </summary>
public sealed record SearchBenefitAllocationsRequest(
    [property: DefaultValue(null)] DefaultIdType? EnrollmentId = null,
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? AllocationType = null,
    [property: DefaultValue(null)] DateTime? FromDate = null,
    [property: DefaultValue(null)] DateTime? ToDate = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<BenefitAllocationDto>>;

/// <summary>
/// DTO for benefit allocation search results.
/// </summary>
public sealed record BenefitAllocationDto(
    DefaultIdType Id,
    DefaultIdType EnrollmentId,
    string EmployeeName,
    string BenefitName,
    DateTime AllocationDate,
    decimal AllocatedAmount,
    string AllocationType,
    string Status);

