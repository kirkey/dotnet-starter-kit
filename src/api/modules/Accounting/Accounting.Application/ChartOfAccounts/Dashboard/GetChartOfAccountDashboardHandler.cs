using Accounting.Application.Dashboard;

namespace Accounting.Application.ChartOfAccounts.Dashboard;

/// <summary>
/// Query to retrieve comprehensive dashboard data for a specific chart of account.
/// </summary>
public sealed record GetChartOfAccountDashboardQuery(Guid AccountId) : IRequest<ChartOfAccountDashboardResponse>;

public sealed class GetChartOfAccountDashboardHandler(
    IReadRepository<ChartOfAccount> chartOfAccountRepository,
    IReadRepository<JournalEntryLine> journalEntryLineRepository,
    IReadRepository<JournalEntry> journalEntryRepository,
    ICacheService cacheService,
    ILogger<GetChartOfAccountDashboardHandler> logger)
    : IRequestHandler<GetChartOfAccountDashboardQuery, ChartOfAccountDashboardResponse>
{
    public async Task<ChartOfAccountDashboardResponse> Handle(GetChartOfAccountDashboardQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"chartofaccount-dashboard:{request.AccountId}";

        var cachedResult = await cacheService.GetAsync<ChartOfAccountDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            return cachedResult;
        }

        logger.LogInformation("Generating dashboard for chart of account {AccountId}", request.AccountId);

        var account = await chartOfAccountRepository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new NotFoundException($"Chart of Account {request.AccountId} not found");

        var today = DateTime.UtcNow.Date;
        var startOfYear = new DateTime(today.Year, 1, 1);
        var startOfMonth = new DateTime(today.Year, today.Month, 1);
        var startOfLastMonth = startOfMonth.AddMonths(-1);
        var lastYearStart = startOfYear.AddYears(-1);

        // Get journal entry lines for this account
        var journalEntryLines = await journalEntryLineRepository.ListAsync(
            new JournalEntryLinesByAccountSpec(request.AccountId), cancellationToken);

        // Get related journal entries for dates and metadata
        var journalEntryIds = journalEntryLines.Select(l => l.JournalEntryId).Distinct().ToList();
        var journalEntries = journalEntryIds.Count > 0
            ? await journalEntryRepository.ListAsync(new JournalEntriesByIdsSpec(journalEntryIds), cancellationToken)
            : new List<JournalEntry>();
        var journalEntryDict = journalEntries.ToDictionary(je => je.Id, je => je);

        // Get sub-accounts if this is a control account
        var subAccounts = account.IsControlAccount
            ? await chartOfAccountRepository.ListAsync(new SubAccountsSpec(request.AccountId), cancellationToken)
            : new List<ChartOfAccount>();

        // Calculate balance metrics
        var balances = CalculateBalanceMetrics(account, journalEntryLines, journalEntryDict, startOfYear, startOfMonth, lastYearStart);

        // Calculate activity metrics
        var activity = CalculateActivityMetrics(journalEntryLines, journalEntryDict, startOfYear, startOfMonth, startOfLastMonth, today);

        // Calculate sub-account summary
        var subAccountSummary = CalculateSubAccountSummary(subAccounts, account.Balance);

        // Generate trends
        var balanceTrend = GenerateBalanceTrend(account.Balance, journalEntryLines, journalEntryDict, 12);
        var debitTrend = GenerateDebitCreditTrend(journalEntryLines, journalEntryDict, true, 12);
        var creditTrend = GenerateDebitCreditTrend(journalEntryLines, journalEntryDict, false, 12);

        // Period activity breakdown
        var periodActivity = CalculatePeriodActivity(journalEntryLines, journalEntryDict, 6);

        // Recent transactions
        var recentTransactions = journalEntryLines
            .Select(l => new { Line = l, JournalEntry = journalEntryDict.GetValueOrDefault(l.JournalEntryId) })
            .Where(x => x.JournalEntry != null)
            .OrderByDescending(x => x.JournalEntry!.Date)
            .Take(10)
            .Select(x => new RecentJournalEntryInfo
            {
                JournalEntryId = x.JournalEntry!.Id,
                JournalEntryLineId = x.Line.Id,
                ReferenceNumber = x.JournalEntry.ReferenceNumber,
                Date = x.JournalEntry.Date,
                DebitAmount = x.Line.DebitAmount,
                CreditAmount = x.Line.CreditAmount,
                Memo = x.Line.Memo,
                Source = x.JournalEntry.Source,
                IsPosted = x.JournalEntry.IsPosted
            }).ToList();

        // Monthly performance
        var monthlyPerformance = GenerateMonthlyPerformance(journalEntryLines, journalEntryDict, 6);

        // Alerts
        var alerts = GenerateAlerts(account, journalEntryLines, journalEntryDict, today);

        var response = new ChartOfAccountDashboardResponse
        {
            AccountId = account.Id,
            AccountCode = account.AccountCode,
            AccountName = account.AccountName,
            AccountType = account.AccountType,
            UsoaCategory = account.UsoaCategory,
            NormalBalance = account.NormalBalance,
            IsActive = account.IsActive,
            IsControlAccount = account.IsControlAccount,
            AllowDirectPosting = account.AllowDirectPosting,
            AccountLevel = account.AccountLevel,
            ParentCode = account.ParentCode,
            ParentAccountId = account.ParentAccountId,
            Balances = balances,
            Activity = activity,
            SubAccounts = subAccountSummary,
            BalanceTrend = balanceTrend,
            DebitTrend = debitTrend,
            CreditTrend = creditTrend,
            PeriodActivity = periodActivity,
            RecentTransactions = recentTransactions,
            MonthlyPerformance = monthlyPerformance,
            Alerts = alerts
        };

        await cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);

        return response;
    }

    private static AccountBalanceMetrics CalculateBalanceMetrics(
        ChartOfAccount account,
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        DateTime startOfYear,
        DateTime startOfMonth,
        DateTime lastYearStart)
    {
        var ytdLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= startOfYear).ToList();

        var ytdDebits = ytdLines.Sum(l => l.DebitAmount);
        var ytdCredits = ytdLines.Sum(l => l.CreditAmount);
        var ytdNet = ytdDebits - ytdCredits;

        // Estimate beginning balance (current - YTD net change)
        var beginningBalance = account.Balance - ytdNet;

        // Estimate last month balance
        var lastMonthLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= startOfMonth).ToList();
        var lastMonthNet = lastMonthLines.Sum(l => l.DebitAmount) - lastMonthLines.Sum(l => l.CreditAmount);
        var lastMonthBalance = account.Balance - lastMonthNet;

        // Estimate last year balance
        var lastYearLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= lastYearStart).ToList();
        var lastYearNet = lastYearLines.Sum(l => l.DebitAmount) - lastYearLines.Sum(l => l.CreditAmount);
        var lastYearBalance = account.Balance - lastYearNet;

        var changePercentage = lastYearBalance != 0
            ? (account.Balance - lastYearBalance) / Math.Abs(lastYearBalance) * 100
            : 0;

        return new AccountBalanceMetrics
        {
            CurrentBalance = account.Balance,
            BeginningBalance = beginningBalance,
            YtdDebits = ytdDebits,
            YtdCredits = ytdCredits,
            YtdNetChange = ytdNet,
            LastMonthBalance = lastMonthBalance,
            LastYearBalance = lastYearBalance,
            BalanceChangePercentage = changePercentage
        };
    }

    private static AccountActivityMetrics CalculateActivityMetrics(
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        DateTime startOfYear,
        DateTime startOfMonth,
        DateTime startOfLastMonth,
        DateTime today)
    {
        var ytdLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= startOfYear).ToList();

        var thisMonthLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= startOfMonth).ToList();

        var lastMonthLines = lines.Where(l =>
            journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
            je.Date >= startOfLastMonth && je.Date < startOfMonth).ToList();

        var avgAmount = lines.Count > 0
            ? lines.Average(l => Math.Abs(l.DebitAmount - l.CreditAmount))
            : 0;

        var largestDebit = lines.Count > 0 ? lines.Max(l => l.DebitAmount) : 0;
        var largestCredit = lines.Count > 0 ? lines.Max(l => l.CreditAmount) : 0;

        DateTime? lastTransactionDate = null;
        var orderedLines = lines
            .Select(l => new { Line = l, JournalEntry = journalEntryDict.GetValueOrDefault(l.JournalEntryId) })
            .Where(x => x.JournalEntry != null)
            .OrderByDescending(x => x.JournalEntry!.Date)
            .FirstOrDefault();

        if (orderedLines?.JournalEntry != null)
        {
            lastTransactionDate = orderedLines.JournalEntry.Date;
        }

        var daysSinceLastTransaction = lastTransactionDate.HasValue
            ? (int)(today - lastTransactionDate.Value.Date).TotalDays
            : 0;

        return new AccountActivityMetrics
        {
            TotalTransactions = lines.Count,
            TransactionsYTD = ytdLines.Count,
            TransactionsThisMonth = thisMonthLines.Count,
            TransactionsLastMonth = lastMonthLines.Count,
            AverageTransactionAmount = avgAmount,
            LargestDebit = largestDebit,
            LargestCredit = largestCredit,
            LastTransactionDate = lastTransactionDate,
            DaysSinceLastTransaction = daysSinceLastTransaction
        };
    }

    private static SubAccountSummary CalculateSubAccountSummary(List<ChartOfAccount> subAccounts, decimal parentBalance)
    {
        var activeSubAccounts = subAccounts.Where(s => s.IsActive).ToList();
        var combinedBalance = subAccounts.Sum(s => s.Balance);

        var topSubAccounts = subAccounts
            .OrderByDescending(s => Math.Abs(s.Balance))
            .Take(5)
            .Select(s => new SubAccountInfo
            {
                AccountId = s.Id,
                AccountCode = s.AccountCode,
                AccountName = s.AccountName,
                Balance = s.Balance,
                PercentageOfParent = parentBalance != 0 ? s.Balance / Math.Abs(parentBalance) * 100 : 0
            }).ToList();

        return new SubAccountSummary
        {
            TotalSubAccounts = subAccounts.Count,
            ActiveSubAccounts = activeSubAccounts.Count,
            CombinedBalance = combinedBalance,
            TopSubAccounts = topSubAccounts
        };
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateBalanceTrend(
        decimal currentBalance,
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        // Work backwards from current balance
        decimal runningBalance = currentBalance;
        var monthlyChanges = new List<(DateTime MonthStart, decimal NetChange)>();

        for (int i = 0; i < months; i++)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthNet = lines
                .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                           je.Date >= monthStart && je.Date < monthEnd)
                .Sum(l => l.DebitAmount - l.CreditAmount);

            monthlyChanges.Add((monthStart, monthNet));
        }

        // Reverse to go from oldest to newest
        monthlyChanges.Reverse();

        // Calculate starting balance (current - all changes)
        var startBalance = currentBalance - monthlyChanges.Sum(m => m.NetChange);
        var balance = startBalance;

        foreach (var (monthStart, netChange) in monthlyChanges)
        {
            balance += netChange;
            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = balance,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<AccountingTimeSeriesDataPoint> GenerateDebitCreditTrend(
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        bool isDebit,
        int months)
    {
        var result = new List<AccountingTimeSeriesDataPoint>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthValue = lines
                .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                           je.Date >= monthStart && je.Date < monthEnd)
                .Sum(l => isDebit ? l.DebitAmount : l.CreditAmount);

            result.Add(new AccountingTimeSeriesDataPoint
            {
                Date = monthStart,
                Value = monthValue,
                Label = monthStart.ToString("MMM yyyy")
            });
        }

        return result;
    }

    private static List<PeriodBreakdown> CalculatePeriodActivity(
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        int months)
    {
        var result = new List<PeriodBreakdown>();
        var today = DateTime.UtcNow.Date;
        var totalAmount = lines.Sum(l => l.DebitAmount + l.CreditAmount);

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            var monthLines = lines
                .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                           je.Date >= monthStart && je.Date < monthEnd)
                .ToList();

            var monthAmount = monthLines.Sum(l => l.DebitAmount + l.CreditAmount);

            result.Add(new PeriodBreakdown
            {
                PeriodName = monthStart.ToString("MMM yyyy"),
                Amount = monthAmount,
                Percentage = totalAmount > 0 ? monthAmount / totalAmount * 100 : 0,
                TransactionCount = monthLines.Count
            });
        }

        return result;
    }

    private static List<MonthlyComparisonData> GenerateMonthlyPerformance(
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        int months)
    {
        var result = new List<MonthlyComparisonData>();
        var today = DateTime.UtcNow.Date;

        for (int i = months - 1; i >= 0; i--)
        {
            var monthStart = new DateTime(today.Year, today.Month, 1).AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);
            var prevMonthStart = monthStart.AddMonths(-1);

            var monthLines = lines
                .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                           je.Date >= monthStart && je.Date < monthEnd)
                .ToList();

            var prevMonthLines = lines
                .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                           je.Date >= prevMonthStart && je.Date < monthStart)
                .ToList();

            var monthAmount = monthLines.Sum(l => l.DebitAmount + l.CreditAmount);
            var prevAmount = prevMonthLines.Sum(l => l.DebitAmount + l.CreditAmount);

            var change = prevAmount > 0 ? (monthAmount - prevAmount) / prevAmount * 100 : 0;

            result.Add(new MonthlyComparisonData
            {
                Month = monthStart.ToString("MMMM"),
                Year = monthStart.Year,
                Amount = monthAmount,
                PreviousAmount = prevAmount,
                ChangePercentage = change,
                TransactionCount = monthLines.Count
            });
        }

        return result;
    }

    private static List<DashboardAlert> GenerateAlerts(
        ChartOfAccount account,
        List<JournalEntryLine> lines,
        Dictionary<Guid, JournalEntry> journalEntryDict,
        DateTime today)
    {
        var alerts = new List<DashboardAlert>();

        // Inactive account alert
        if (!account.IsActive)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Inactive Account",
                Severity = "Warning",
                Message = $"Account {account.AccountCode} - {account.AccountName} is inactive",
                CreatedDate = today,
                RelatedEntityId = account.Id,
                RelatedEntityName = account.AccountName
            });
        }

        // Control account with direct postings
        if (account.IsControlAccount && lines.Count > 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Control Account Postings",
                Severity = "Info",
                Message = $"Control account has {lines.Count} direct postings",
                CreatedDate = today,
                RelatedEntityId = account.Id,
                RelatedEntityName = account.AccountName
            });
        }

        // No recent activity
        var recentLines = lines
            .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                       je.Date >= today.AddDays(-90))
            .ToList();

        if (account.IsActive && lines.Count > 0 && recentLines.Count == 0)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "No Recent Activity",
                Severity = "Info",
                Message = $"Account has no activity in the last 90 days",
                CreatedDate = today,
                RelatedEntityId = account.Id,
                RelatedEntityName = account.AccountName
            });
        }

        // Large balance change
        var ytdNet = lines
            .Where(l => journalEntryDict.TryGetValue(l.JournalEntryId, out var je) &&
                       je.Date >= new DateTime(today.Year, 1, 1))
            .Sum(l => l.DebitAmount - l.CreditAmount);

        if (Math.Abs(ytdNet) > 100000)
        {
            alerts.Add(new DashboardAlert
            {
                AlertType = "Significant Balance Change",
                Severity = "Info",
                Message = $"YTD net change of {ytdNet:C}",
                CreatedDate = today,
                RelatedEntityId = account.Id,
                RelatedEntityName = account.AccountName
            });
        }

        return alerts.Take(5).ToList();
    }
}

// Specification classes
public sealed class JournalEntryLinesByAccountSpec : Specification<JournalEntryLine>
{
    public JournalEntryLinesByAccountSpec(Guid accountId)
    {
        Query.Where(l => l.AccountId == accountId);
    }
}

public sealed class JournalEntriesByIdsSpec : Specification<JournalEntry>
{
    public JournalEntriesByIdsSpec(List<Guid> ids)
    {
        Query.Where(je => ids.Contains(je.Id));
    }
}

public sealed class SubAccountsSpec : Specification<ChartOfAccount>
{
    public SubAccountsSpec(Guid parentAccountId)
    {
        Query.Where(a => a.ParentAccountId == parentAccountId);
    }
}
