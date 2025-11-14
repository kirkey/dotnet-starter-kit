namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Delete.v1;

/// <summary>
/// Command to delete tax bracket.
/// </summary>
public sealed record DeleteTaxBracketCommand(
    DefaultIdType Id
) : IRequest<DeleteTaxBracketResponse>;

/// <summary>
/// Response for tax bracket deletion.
/// </summary>
public sealed record DeleteTaxBracketResponse(
    DefaultIdType Id,
    bool Success);

