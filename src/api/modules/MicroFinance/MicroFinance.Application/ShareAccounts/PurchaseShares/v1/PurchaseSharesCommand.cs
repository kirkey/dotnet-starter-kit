using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;

public sealed record PurchaseSharesCommand(
    Guid ShareAccountId,
    [property: DefaultValue(10)] int NumberOfShares,
    [property: DefaultValue(100)] decimal PricePerShare) : IRequest<PurchaseSharesResponse>;
