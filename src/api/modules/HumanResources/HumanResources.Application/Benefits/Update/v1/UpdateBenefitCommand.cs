namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Command to update a benefit.
/// </summary>
public sealed record UpdateBenefitCommand(
    DefaultIdType Id,
    decimal? EmployeeContribution = null,
    decimal? EmployerContribution = null,
    string? CoverageType = null,
    string? ProviderName = null,
    decimal? CoverageAmount = null,
    string? Description = null,
    bool? IsActive = null) : IRequest<UpdateBenefitResponse>;

/// <summary>
/// Response for update benefit.
/// </summary>
public sealed record UpdateBenefitResponse(
    DefaultIdType Id,
    string BenefitName,
    bool IsActive);

