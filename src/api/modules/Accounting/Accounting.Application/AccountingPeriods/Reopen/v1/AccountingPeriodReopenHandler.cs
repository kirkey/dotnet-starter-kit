using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Reopen.v1;

/// <summary>
/// Handler to reopen an accounting period.
/// </summary>
public sealed class AccountingPeriodReopenHandler(IRepository<AccountingPeriod> repository)
    : IRequestHandler<AccountingPeriodReopenCommand, AccountingPeriodTransitionResponse>
{
    public async Task<AccountingPeriodTransitionResponse> Handle(AccountingPeriodReopenCommand request, CancellationToken cancellationToken)
    {
        var period = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Accounting period {request.Id} not found");

        period.Reopen();
        await repository.UpdateAsync(period, cancellationToken);

        return new AccountingPeriodTransitionResponse(period.Id, period.Name, period.IsClosed);
    }
}
