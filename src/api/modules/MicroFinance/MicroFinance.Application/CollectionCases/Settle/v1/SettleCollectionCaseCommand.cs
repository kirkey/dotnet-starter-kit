using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Settle.v1;

public sealed record SettleCollectionCaseCommand(
    Guid Id,
    decimal SettlementAmount,
    string Terms) : IRequest<SettleCollectionCaseResponse>;
