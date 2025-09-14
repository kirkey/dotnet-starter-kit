namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Renew.v1;

public sealed record RenewWholesaleContractCommand(DefaultIdType Id, DateTime NewEndDate) : IRequest<RenewWholesaleContractResponse>;

public sealed record RenewWholesaleContractResponse(DefaultIdType Id);

