using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Get.v1;

public sealed record GetBranchRequest(DefaultIdType Id) : IRequest<BranchResponse>;
