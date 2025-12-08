using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.ApplyPayment.v1;

public sealed record ApplyPaymentCommand(
    DefaultIdType ScheduleId,
    decimal Amount,
    DateOnly PaymentDate) : IRequest<ApplyPaymentResponse>;
