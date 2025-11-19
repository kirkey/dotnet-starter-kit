namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Response for deleting a tax bracket.
/// </summary>
/// <param name="Id">The identifier of the deleted tax bracket.</param>
public sealed record DeleteTaxResponse(DefaultIdType Id);

