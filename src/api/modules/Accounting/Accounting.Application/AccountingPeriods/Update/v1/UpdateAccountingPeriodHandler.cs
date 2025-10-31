namespace Accounting.Application.AccountingPeriods.Update.v1;

/// <summary>
/// Handler for <see cref="UpdateAccountingPeriodCommand"/> which applies allowed updates to an existing AccountingPeriod.
/// Enforces domain invariants via the entity's Update method and persists changes.
/// </summary>
public sealed class UpdateAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<UpdateAccountingPeriodCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the update command. Throws <see cref="Exceptions.AccountingPeriodNotFoundException"/> if the period does not exist.
    /// </summary>
    /// <param name="request">The update command containing values to apply.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The identifier of the updated accounting period.</returns>
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
