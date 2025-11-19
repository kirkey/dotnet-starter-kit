namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Command to create a new benefit per Philippines Labor Code.
/// Supports mandatory (SSS, PhilHealth, Pag-IBIG) and optional benefits.
/// </summary>
public sealed record CreateBenefitCommand(
    [property: DefaultValue("Health Insurance")] string BenefitName,
    [property: DefaultValue("Health")] string BenefitType,
    [property: DefaultValue(0)] decimal EmployeeContribution = 0,
    [property: DefaultValue(0)] decimal EmployerContribution = 0,
    [property: DefaultValue(false)] bool IsMandatory = false,
    [property: DefaultValue(null)] string? CoverageType = null,
    [property: DefaultValue(null)] string? ProviderName = null,
    [property: DefaultValue(null)] decimal? CoverageAmount = null,
    [property: DefaultValue(null)] int? WaitingPeriodDays = null,
    [property: DefaultValue(null)] string? Description = null
) : IRequest<CreateBenefitResponse>;

/// <summary>
/// Response for benefit creation.
/// </summary>
public sealed record CreateBenefitResponse(
    DefaultIdType Id,
    string BenefitName,
    string BenefitType,
    bool IsMandatory,
    bool IsActive);

