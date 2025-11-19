namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Get.v1;

/// <summary>
/// Query to retrieve a tax master configuration by ID.
/// </summary>
public sealed record GetTaxRequest(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id) : IRequest<TaxResponse>;

