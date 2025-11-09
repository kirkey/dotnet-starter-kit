using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Close.v1;

/// <summary>
/// Handler to close an accounting period.
/// </summary>
public sealed class AccountingPeriodCloseHandler(IRepository<AccountingPeriod> repository)
    : IRequestHandler<AccountingPeriodCloseCommand, AccountingPeriodTransitionResponse>
{
    public async Task<AccountingPeriodTransitionResponse> Handle(AccountingPeriodCloseCommand request, CancellationToken cancellationToken)
    {
        var period = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Accounting period {request.Id} not found");

        period.Close();
        await repository.UpdateAsync(period, cancellationToken);

        return new AccountingPeriodTransitionResponse(period.Id, period.Name, period.IsClosed);
    }
}
