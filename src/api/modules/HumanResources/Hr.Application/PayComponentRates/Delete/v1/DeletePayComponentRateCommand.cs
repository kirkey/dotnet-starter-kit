namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Delete.v1;

public sealed record DeletePayComponentRateCommand(DefaultIdType Id) : IRequest<DeletePayComponentRateResponse>;

