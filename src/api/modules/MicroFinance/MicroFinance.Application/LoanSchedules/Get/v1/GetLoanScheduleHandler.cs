using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;

public sealed class GetLoanScheduleHandler(
    [FromKeyedServices("microfinance:loanschedules")] IRepository<LoanSchedule> repository)
    : IRequestHandler<GetLoanScheduleRequest, LoanScheduleResponse>
{
    public async Task<LoanScheduleResponse> Handle(GetLoanScheduleRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = await repository.FirstOrDefaultAsync(
            new LoanScheduleByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (schedule is null)
        {
            throw new NotFoundException($"Loan schedule with ID {request.Id} not found.");
        }

        return new LoanScheduleResponse(
            schedule.Id,
            schedule.LoanId,
            schedule.InstallmentNumber,
            schedule.DueDate,
            schedule.PrincipalAmount,
            schedule.InterestAmount,
            schedule.TotalAmount,
            schedule.PaidAmount,
            schedule.IsPaid,
            schedule.PaidDate
        );
    }
}
