using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;

public sealed record GetShareAccountRequest(DefaultIdType Id) : IRequest<ShareAccountResponse>;
