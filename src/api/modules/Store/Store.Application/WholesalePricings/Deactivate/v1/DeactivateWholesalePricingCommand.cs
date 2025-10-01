namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Deactivate.v1;

public sealed record DeactivateWholesalePricingCommand(DefaultIdType Id) : IRequest<DeactivateWholesalePricingResponse>;

public sealed record DeactivateWholesalePricingResponse(DefaultIdType Id);

