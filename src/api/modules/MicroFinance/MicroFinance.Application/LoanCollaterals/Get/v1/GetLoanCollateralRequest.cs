using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Get.v1;

public sealed record GetLoanCollateralRequest(DefaultIdType Id) : IRequest<LoanCollateralResponse>;
