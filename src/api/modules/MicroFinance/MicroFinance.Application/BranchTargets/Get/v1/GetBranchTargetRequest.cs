using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Get.v1;

/// <summary>
/// Request to get a branch target by ID.
/// </summary>
public sealed record GetBranchTargetRequest(Guid Id) : IRequest<BranchTargetResponse>;
