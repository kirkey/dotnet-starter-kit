namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Command to delete a tax bracket.
/// </summary>
public sealed record DeleteTaxCommand(DefaultIdType Id) : IRequest<DeleteTaxResponse>;

