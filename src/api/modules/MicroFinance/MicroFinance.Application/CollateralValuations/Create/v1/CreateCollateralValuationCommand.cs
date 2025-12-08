using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Create.v1;

public sealed record CreateCollateralValuationCommand(
    DefaultIdType CollateralId,
    string ValuationReference,
    DateOnly ValuationDate,
    string ValuationMethod,
    decimal MarketValue,
    decimal ForcedSaleValue,
    decimal InsurableValue,
    string? AppraiserName = null,
    string? AppraiserCompany = null,
    decimal? PreviousValue = null) : IRequest<CreateCollateralValuationResponse>;
