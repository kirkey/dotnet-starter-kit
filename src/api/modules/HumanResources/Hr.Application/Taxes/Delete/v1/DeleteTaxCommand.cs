namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Command to delete a tax master configuration.
/// </summary>
public sealed record DeleteTaxCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id) : IRequest<DefaultIdType>;

