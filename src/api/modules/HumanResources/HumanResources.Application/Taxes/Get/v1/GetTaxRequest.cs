namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Request to get a tax bracket by its identifier.
/// </summary>
public sealed record GetTaxRequest(DefaultIdType Id) : IRequest<TaxResponse>;

