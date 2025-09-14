namespace FSH.Starter.WebApi.Store.Application.WholesaleContracts.Actions.Terminate.v1;

public sealed record TerminateWholesaleContractCommand(DefaultIdType Id, string Reason) : IRequest<TerminateWholesaleContractResponse>;

public sealed record TerminateWholesaleContractResponse(DefaultIdType Id);

