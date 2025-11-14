namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Command to update benefit details.
/// </summary>
public sealed record UpdateBenefitCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] decimal? EmployeeContribution = null,
    [property: DefaultValue(null)] decimal? EmployerContribution = null,
    [property: DefaultValue(null)] string? CoverageType = null,
    [property: DefaultValue(null)] string? ProviderName = null,
    [property: DefaultValue(null)] decimal? CoverageAmount = null,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] bool? IsActive = null
) : IRequest<UpdateBenefitResponse>;

/// <summary>
/// Response for benefit update.
/// </summary>
public sealed record UpdateBenefitResponse(
    DefaultIdType Id,
    string BenefitName,
    bool IsActive);

