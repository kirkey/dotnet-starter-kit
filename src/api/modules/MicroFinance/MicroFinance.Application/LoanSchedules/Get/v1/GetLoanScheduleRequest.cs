using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;

public sealed record GetLoanScheduleRequest(Guid Id) : IRequest<LoanScheduleResponse>;
