using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Get.v1;

public sealed record GetCashVaultRequest(Guid Id) : IRequest<CashVaultResponse>;
