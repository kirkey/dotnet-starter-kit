using MediatR;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;

/// <summary>
/// Request to get designation by ID.
/// </summary>
public sealed record GetDesignationRequest(DefaultIdType Id) : IRequest<DesignationResponse>;

