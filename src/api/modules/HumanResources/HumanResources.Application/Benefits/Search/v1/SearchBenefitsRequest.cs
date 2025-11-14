namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;

/// <summary>
/// Request to search benefits.
/// </summary>
public sealed record SearchBenefitsRequest(
    [property: DefaultValue(null)] string? BenefitType = null,
    [property: DefaultValue(null)] bool? IsMandatory = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    [property: DefaultValue(null)] string? SearchTerm = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<BenefitDto>>;

/// <summary>
/// DTO for benefit search results.
/// </summary>
public sealed record BenefitDto(
    DefaultIdType Id,
    string BenefitName,
    string BenefitType,
    decimal EmployeeContribution,
    decimal EmployerContribution,
    bool IsMandatory,
    bool IsActive);

