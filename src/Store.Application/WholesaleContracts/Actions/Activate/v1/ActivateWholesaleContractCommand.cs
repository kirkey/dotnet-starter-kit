namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Activate.v1;

public sealed record ActivateWholesaleContractCommand(DefaultIdType Id) : IRequest<ActivateWholesaleContractResponse>;

public sealed record ActivateWholesaleContractResponse(DefaultIdType Id);

