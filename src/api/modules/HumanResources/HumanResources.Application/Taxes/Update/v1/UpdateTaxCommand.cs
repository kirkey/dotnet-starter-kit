namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Command to update a tax bracket.
/// </summary>
public sealed record UpdateTaxCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? FilingStatus = null,
    [property: DefaultValue(null)] string? Description = null) : IRequest<UpdateTaxResponse>;

