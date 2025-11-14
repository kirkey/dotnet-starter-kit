namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;

/// <summary>
/// Response for deleting a benefit.
/// </summary>
/// <param name="Id">The identifier of the deleted benefit.</param>
public sealed record DeleteBenefitResponse(DefaultIdType Id);

