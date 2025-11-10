using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Close.v1;

/// <summary>
/// Handler to close an accounting period.
/// </summary>
public sealed class AccountingPeriodCloseHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<AccountingPeriodCloseCommand, AccountingPeriodTransitionResponse>
{
    public async Task<AccountingPeriodTransitionResponse> Handle(AccountingPeriodCloseCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var period = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Accounting period {request.Id} not found");

        period.Close();
        await repository.UpdateAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AccountingPeriodTransitionResponse(period.Id, period.Name, period.IsClosed);
    }
}
