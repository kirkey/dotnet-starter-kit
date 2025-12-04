using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Get.v1;

/// <summary>
/// Request to get a collection action by ID.
/// </summary>
public sealed record GetCollectionActionRequest(Guid Id) : IRequest<CollectionActionResponse>;
