namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;

/// <summary>
/// Response for updating a benefit.
/// </summary>
/// <param name="Id">The identifier of the updated benefit.</param>
public sealed record UpdateBenefitResponse(DefaultIdType Id);

