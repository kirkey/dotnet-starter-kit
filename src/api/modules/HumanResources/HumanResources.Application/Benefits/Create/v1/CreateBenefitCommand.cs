namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Command to create a new benefit.
/// </summary>
public sealed record CreateBenefitCommand(
    [property: DefaultValue("Health Insurance")] string BenefitName,
    [property: DefaultValue("Health")] string BenefitType,
    [property: DefaultValue(500)] decimal EmployeeContribution = 500,
    [property: DefaultValue(500)] decimal EmployerContribution = 500,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] decimal? AnnualLimit = null,
    [property: DefaultValue(false)] bool IsCarryoverAllowed = false,
    [property: DefaultValue(null)] int? MinimumEligibleEmployees = null) : IRequest<CreateBenefitResponse>;

