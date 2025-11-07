namespace Accounting.Application.TrialBalance.Create.v1;

/// <summary>
/// Handler for creating a new trial balance report.
/// Optionally auto-generates line items from General Ledger.
/// </summary>
public sealed class TrialBalanceCreateHandler(
    IRepository<Domain.Entities.TrialBalance> repository,
    IReadRepository<GeneralLedger> glRepository,
    IReadRepository<ChartOfAccount> accountRepository,
    ILogger<TrialBalanceCreateHandler> logger)
    : IRequestHandler<TrialBalanceCreateCommand, TrialBalanceCreateResponse>
{
    private readonly IRepository<Domain.Entities.TrialBalance> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IReadRepository<GeneralLedger> _glRepository = glRepository ?? throw new ArgumentNullException(nameof(glRepository));
    private readonly IReadRepository<ChartOfAccount> _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    private readonly ILogger<TrialBalanceCreateHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TrialBalanceCreateResponse> Handle(TrialBalanceCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Creating trial balance {TrialBalanceNumber} for period {PeriodId}",
            request.TrialBalanceNumber, request.PeriodId);

        // Check if trial balance number already exists
        var existing = await _repository.ListAsync(cancellationToken);
        if (existing.Any(tb => tb.TrialBalanceNumber.Equals(request.TrialBalanceNumber, StringComparison.OrdinalIgnoreCase)))
        {
            _logger.LogWarning("Trial balance number {TrialBalanceNumber} already exists", request.TrialBalanceNumber);
            throw new InvalidOperationException($"Trial balance number '{request.TrialBalanceNumber}' already exists.");
        }

        // Create the trial balance entity
        var trialBalance = Domain.Entities.TrialBalance.Create(
            request.TrialBalanceNumber,
            request.PeriodId,
            request.PeriodStartDate,
            request.PeriodEndDate,
            request.IncludeZeroBalances,
            request.Description,
            request.Notes
        );

        // Auto-generate line items from GL if requested
        if (request.AutoGenerate)
        {
            _logger.LogInformation("Auto-generating trial balance line items from General Ledger");
            await GenerateLineItemsFromGL(trialBalance, request.PeriodStartDate, request.PeriodEndDate, cancellationToken);
        }

        await _repository.AddAsync(trialBalance, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Trial balance {TrialBalanceNumber} created successfully with {AccountCount} accounts",
            trialBalance.TrialBalanceNumber, trialBalance.AccountCount);

        return new TrialBalanceCreateResponse
        {
            Id = trialBalance.Id,
            TrialBalanceNumber = trialBalance.TrialBalanceNumber,
            GeneratedDate = trialBalance.GeneratedDate,
            TotalDebits = trialBalance.TotalDebits,
            TotalCredits = trialBalance.TotalCredits,
            IsBalanced = trialBalance.IsBalanced,
            Status = trialBalance.Status,
            AccountCount = trialBalance.AccountCount
        };
    }

    private async Task GenerateLineItemsFromGL(Domain.Entities.TrialBalance trialBalance, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        // Get all accounts
        var accounts = await _accountRepository.ListAsync(cancellationToken);

        // Get GL entries for the period
        var glEntries = await _glRepository.ListAsync(cancellationToken);
        var periodEntries = glEntries.Where(gl => gl.TransactionDate >= startDate && gl.TransactionDate <= endDate);

        // Group by account and calculate balances
        var accountBalances = periodEntries
            .GroupBy(gl => gl.AccountId)
            .Select(g => new
            {
                AccountId = g.Key,
                TotalDebits = g.Sum(gl => gl.Debit),
                TotalCredits = g.Sum(gl => gl.Credit)
            });

        foreach (var balance in accountBalances)
        {
            var account = accounts.FirstOrDefault(a => a.Id == balance.AccountId);
            if (account == null) continue;

            trialBalance.AddLineItem(
                account.AccountCode,
                account.AccountName,
                account.AccountType,
                balance.TotalDebits,
                balance.TotalCredits
            );
        }
    }
}
