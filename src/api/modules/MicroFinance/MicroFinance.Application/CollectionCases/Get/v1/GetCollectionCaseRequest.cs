using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;

public sealed record GetCollectionCaseRequest(Guid Id) : IRequest<CollectionCaseResponse>;
