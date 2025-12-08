using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.ApplyPayment.v1;

public sealed class ApplyPaymentHandler(
    [FromKeyedServices("microfinance:loanschedules")] IRepository<LoanSchedule> repository,
    ILogger<ApplyPaymentHandler> logger)
    : IRequestHandler<ApplyPaymentCommand, ApplyPaymentResponse>
{
    public async Task<ApplyPaymentResponse> Handle(
        ApplyPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var schedule = await repository.FirstOrDefaultAsync(
            new LoanScheduleByIdSpec(request.ScheduleId), cancellationToken).ConfigureAwait(false);

        if (schedule is null)
        {
            throw new NotFoundException($"Loan schedule with ID {request.ScheduleId} not found.");
        }

        schedule.ApplyPayment(request.Amount, request.PaymentDate);

        await repository.UpdateAsync(schedule, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Payment of {Amount} applied to schedule {ScheduleId}",
            request.Amount, request.ScheduleId);

        return new ApplyPaymentResponse(
            schedule.Id,
            schedule.PaidAmount,
            schedule.IsPaid,
            schedule.PaidDate);
    }
}
