using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;

public sealed record GetLoanCollateralRequest(Guid Id) : IRequest<LoanCollateralResponse>;
