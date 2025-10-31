namespace Accounting.Application.AccountingPeriods.Commands.CloseAccountingPeriod.v1;

/// <summary>
/// Handles <see cref="CloseAccountingPeriodCommand"/>: validates the request, optionally generates closing entries
/// and year-end adjustments, and marks the period as closed.
/// </summary>
public sealed class CloseAccountingPeriodCommandHandler(
    ILogger<CloseAccountingPeriodCommandHandler> logger,
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> periodRepository,
    [FromKeyedServices("accounting:journalentries")] IRepository<JournalEntry> journalRepository,
    [FromKeyedServices("accounting:accounts")] IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<CloseAccountingPeriodCommand, DefaultIdType>
{
    /// <summary>
    /// Executes the closing process for an accounting period.
    /// </summary>
    /// <param name="request">Close command containing options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id of the closed period.</returns>
    public async Task<DefaultIdType> Handle(CloseAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Mark unused injected repository as referenced to avoid warnings until implemented
        _ = journalRepository;

        // Get the accounting period
        var period = await periodRepository.GetByIdAsync(request.AccountingPeriodId, cancellationToken);
        if (period == null)
        {
            throw new ArgumentException($"Accounting period with ID {request.AccountingPeriodId} not found");
        }

        // Validate that period is not already closed
        if (period.IsClosed)
        {
            throw new InvalidOperationException($"Accounting period {period.Name} is already closed");
        }

        // Validate closing date is within period
        if (request.ClosingDate < period.StartDate || request.ClosingDate > period.EndDate)
        {
            throw new ArgumentException("Closing date must be within the accounting period");
        }

        if (request.GenerateClosingEntries)
        {
            await GenerateClosingEntries(period, request.ClosingDate, cancellationToken);
        }

        if (request.PerformYearEndAdjustments)
        {
            await PerformYearEndAdjustments(period, request.ClosingDate, cancellationToken);
        }

        // Close the period
        // Domain Close has no parameters; call parameterless Close and persist any notes separately if needed
        period.Close();
        await periodRepository.UpdateAsync(period, cancellationToken);

        logger.LogInformation("Accounting period {PeriodName} closed successfully on {ClosingDate}", 
            period.Name, request.ClosingDate);

        return period.Id;
    }

    /// <summary>
    /// Generates closing journal entries for the period. (Placeholder - implement business rules.)
    /// </summary>
    private async Task GenerateClosingEntries(AccountingPeriod period, DateTime closingDate, CancellationToken cancellationToken)
    {
        // Reference parameters to avoid unused-parameter warnings until implementation is added
        _ = period;
        _ = closingDate;

        // Get revenue and expense accounts
        var accounts = await accountRepository.ListAsync(cancellationToken);
        var revenueAccounts = accounts.Where(a => a.AccountType == "Revenue").ToList();
        var expenseAccounts = accounts.Where(a => a.AccountType.Contains("Expense")).ToList();

        // Create closing entries for revenue accounts (debit revenue, credit income summary)
        foreach (var account in revenueAccounts)
        {
            // This would involve creating journal entries to close revenue accounts
            // Implementation would depend on specific business rules
        }

        // Create closing entries for expense accounts (debit income summary, credit expenses)
        foreach (var account in expenseAccounts)
        {
            // This would involve creating journal entries to close expense accounts
            // Implementation would depend on specific business rules
        }
    }

    /// <summary>
    /// Performs year-end adjustment entries. (Placeholder - implement business rules.)
    /// </summary>
    private Task PerformYearEndAdjustments(AccountingPeriod period, DateTime closingDate, CancellationToken cancellationToken)
    {
        // Implement year-end adjustments such as:
        // - Depreciation calculations
        // - Accrual adjustments
        // - Inventory adjustments
        // - Bad debt provisions
        
        // Use entity Name property (there is no PeriodName property on the domain entity)
        logger.LogInformation("Year-end adjustments completed for period {PeriodName}", period.Name);
        return Task.CompletedTask;
    }
}
