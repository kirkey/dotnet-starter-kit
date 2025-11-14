namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;

/// <summary>
/// Response for creating a benefit.
/// </summary>
/// <param name="Id">The identifier of the created benefit.</param>
public sealed record CreateBenefitResponse(DefaultIdType Id);

