namespace Accounting.Application.AccountingPeriods.Update.v1;

public sealed class UpdateAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<UpdateAccountingPeriodCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (period is null) throw new Exceptions.AccountingPeriodNotFoundException(request.Id);

        period.Update(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.FiscalYear,
            request.PeriodType,
            request.IsAdjustmentPeriod,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(period, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return period.Id;
    }
}
