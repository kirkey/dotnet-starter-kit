namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;

public sealed record GetTaxBracketRequest(DefaultIdType Id) : IRequest<TaxBracketResponse>;

