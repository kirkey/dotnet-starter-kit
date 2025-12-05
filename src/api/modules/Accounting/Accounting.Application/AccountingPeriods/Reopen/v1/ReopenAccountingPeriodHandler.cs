using Accounting.Application.AccountingPeriods.Responses;

namespace Accounting.Application.AccountingPeriods.Reopen.v1;

/// <summary>
/// Handler to reopen an accounting period.
/// </summary>
public sealed class ReopenAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<ReopenAccountingPeriodCommand, AccountingPeriodTransitionResponse>
{
    public async Task<AccountingPeriodTransitionResponse> Handle(ReopenAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var period = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Accounting period {request.Id} not found");

        period.Reopen();
        await repository.UpdateAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AccountingPeriodTransitionResponse(period.Id, period.Name, period.IsClosed);
    }
}
