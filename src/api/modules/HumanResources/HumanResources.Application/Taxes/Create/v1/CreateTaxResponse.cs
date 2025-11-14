namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Create.v1;

/// <summary>
/// Response for creating a tax bracket.
/// </summary>
/// <param name="Id">The identifier of the created tax bracket.</param>
public sealed record CreateTaxResponse(DefaultIdType Id);

