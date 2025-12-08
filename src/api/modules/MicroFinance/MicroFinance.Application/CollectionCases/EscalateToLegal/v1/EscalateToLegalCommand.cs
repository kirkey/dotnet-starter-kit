using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.EscalateToLegal.v1;

public sealed record EscalateToLegalCommand(DefaultIdType Id, string Reason) : IRequest<EscalateToLegalResponse>;
