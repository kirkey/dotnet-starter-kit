using Accounting.Domain.Entities;

namespace Accounting.Application.AccountingPeriods.Delete.v1;

/// <summary>
/// Handler that deletes an accounting period after validating existence. Throws if period not found.
/// </summary>
public sealed class DeleteAccountingPeriodHandler(
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<DeleteAccountingPeriodCommand>
{
    /// <summary>
    /// Deletes the specified accounting period.
    /// </summary>
    public async Task Handle(DeleteAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        await repository.DeleteAsync(period, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
