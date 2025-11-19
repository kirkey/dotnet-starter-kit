namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Response for updating a tax bracket.
/// </summary>
/// <param name="Id">The identifier of the updated tax bracket.</param>
public sealed record UpdateTaxResponse(DefaultIdType Id);

