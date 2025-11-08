namespace Accounting.Application.DeferredRevenues.Delete.v1;

public sealed record DeleteDeferredRevenueCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

