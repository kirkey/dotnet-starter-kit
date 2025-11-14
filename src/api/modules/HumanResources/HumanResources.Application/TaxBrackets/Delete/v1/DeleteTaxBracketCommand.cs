namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Delete.v1;

public sealed record DeleteTaxBracketCommand(DefaultIdType Id) : IRequest<DeleteTaxBracketResponse>;

