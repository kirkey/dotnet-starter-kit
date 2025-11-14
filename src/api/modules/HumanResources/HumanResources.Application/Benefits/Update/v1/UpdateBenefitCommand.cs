namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Command to update a benefit.
/// </summary>
public sealed record UpdateBenefitCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? BenefitName = null,
    [property: DefaultValue(null)] decimal? EmployeeContribution = null,
    [property: DefaultValue(null)] decimal? EmployerContribution = null,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] bool? IsRequired = null,
    [property: DefaultValue(null)] bool? IsActive = null) : IRequest<UpdateBenefitResponse>;

