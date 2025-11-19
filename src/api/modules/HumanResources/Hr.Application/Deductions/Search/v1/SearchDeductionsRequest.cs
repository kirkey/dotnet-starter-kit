namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;

/// <summary>
/// Request to search deductions.
/// </summary>
public sealed record SearchDeductionsRequest(
    [property: DefaultValue(null)] string? DeductionType = null,
    [property: DefaultValue(null)] string? RecoveryMethod = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    [property: DefaultValue(null)] string? SearchTerm = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<DeductionDto>>;

/// <summary>
/// DTO for deduction search results.
/// </summary>
public sealed record DeductionDto(
    DefaultIdType Id,
    string DeductionName,
    string DeductionType,
    string RecoveryMethod,
    decimal MaxRecoveryPercentage,
    bool RequiresApproval,
    bool IsRecurring,
    bool IsActive);

