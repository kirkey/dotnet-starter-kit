using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Get.v1;

/// <summary>
/// Request to get a promise to pay by ID.
/// </summary>
public sealed record GetPromiseToPayRequest(DefaultIdType Id) : IRequest<PromiseToPayResponse>;
