using Ardalis.Specification;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Dashboard;

/// <summary>
/// Query to get comprehensive dashboard analytics for a savings product
/// </summary>
public sealed record GetSavingsProductDashboardQuery(Guid ProductId) : IRequest<SavingsProductDashboardResponse>;

/// <summary>
/// Handler for savings product dashboard analytics
/// </summary>
public sealed class GetSavingsProductDashboardHandler(
    IReadRepository<SavingsProduct> productRepository,
    IReadRepository<SavingsAccount> accountRepository,
    IReadRepository<SavingsTransaction> transactionRepository,
    IReadRepository<Member> memberRepository,
    ICacheService cacheService)
    : IRequestHandler<GetSavingsProductDashboardQuery, SavingsProductDashboardResponse>
{
    private const string CacheKeyPrefix = "savings-product-dashboard:";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public async Task<SavingsProductDashboardResponse> Handle(
        GetSavingsProductDashboardQuery request,
        CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeyPrefix}{request.ProductId}";

        var cachedResponse = await cacheService.GetAsync<SavingsProductDashboardResponse>(cacheKey, cancellationToken);
        if (cachedResponse is not null)
        {
            return cachedResponse;
        }

        // Get product
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException($"Savings product with ID {request.ProductId} not found");

        // Get all accounts for this product using specification
        var accountsSpec = new SavingsAccountsByProductSpec(request.ProductId);
        var accounts = await accountRepository.ListAsync(accountsSpec, cancellationToken);

        // Get member information for recent accounts
        var memberIds = accounts.Select(a => a.MemberId).Distinct().ToList();
        var membersSpec = new MembersByIdsForSavingsSpec(memberIds);
        var membersList = memberIds.Count != 0
            ? await memberRepository.ListAsync(membersSpec, cancellationToken)
            : new List<Member>();
        var members = membersList.ToDictionary(m => m.Id, m => $"{m.FirstName} {m.LastName}");

        // Get account IDs for transaction queries
        var accountIds = accounts.Select(a => a.Id).ToList();

        // Get transactions for the last 12 months (using DateOnly)
        var twelveMonthsAgoDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12));
        var thirtyDaysAgoDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));

        var transactionsSpec = new SavingsTransactionsByAccountIdsSpec(accountIds, twelveMonthsAgoDate);
        var transactions = accountIds.Count != 0
            ? await transactionRepository.ListAsync(transactionsSpec, cancellationToken)
            : new List<SavingsTransaction>();

        var recentTransactions = transactions.Where(t => t.TransactionDate >= thirtyDaysAgoDate).ToList();

        // Build dashboard response
        var overview = BuildOverview(product);
        var accountStats = BuildAccountStatistics(accounts);
        var balanceMetrics = BuildBalanceMetrics(accounts, product.MinBalanceForInterest);
        var transactionMetrics = BuildTransactionMetrics(recentTransactions, transactions);
        var interestMetrics = BuildInterestMetrics(accounts, transactions, product.InterestRate);
        var statusDistribution = BuildStatusDistribution(accounts);
        var monthlyTrends = BuildMonthlyTrends(accounts, transactions);
        var recentAccounts = BuildRecentAccounts(accounts, members);
        var alerts = BuildAlerts(accounts, product);

        var response = new SavingsProductDashboardResponse(
            overview,
            accountStats,
            balanceMetrics,
            transactionMetrics,
            interestMetrics,
            statusDistribution,
            monthlyTrends,
            recentAccounts,
            alerts,
            DateTime.UtcNow);

        await cacheService.SetAsync(cacheKey, response, CacheDuration, cancellationToken);

        return response;
    }

    private static SavingsProductOverview BuildOverview(SavingsProduct product)
    {
        return new SavingsProductOverview(
            product.Id,
            product.Code,
            product.Name,
            product.Description,
            product.InterestRate,
            product.InterestCalculation,
            product.InterestPostingFrequency,
            product.MinOpeningBalance,
            product.MinBalanceForInterest,
            product.MinWithdrawalAmount,
            product.MaxWithdrawalPerDay,
            product.AllowOverdraft,
            product.OverdraftLimit,
            product.IsActive);
    }

    private static SavingsProductAccountStatistics BuildAccountStatistics(List<SavingsAccount> accounts)
    {
        var total = accounts.Count;
        var active = accounts.Count(a => a.Status == SavingsAccount.StatusActive);
        var pending = accounts.Count(a => a.Status == SavingsAccount.StatusPending);
        var dormant = accounts.Count(a => a.Status == SavingsAccount.StatusDormant);
        var closed = accounts.Count(a => a.Status == SavingsAccount.StatusClosed);
        var frozen = accounts.Count(a => a.Status == SavingsAccount.StatusFrozen);

        var thirtyDaysAgoDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));
        var twelveMonthsAgoDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12));

        var newLast30Days = accounts.Count(a => a.OpenedDate >= thirtyDaysAgoDate);
        var newLast12Months = accounts.Count(a => a.OpenedDate >= twelveMonthsAgoDate);

        var closureRate = total > 0 ? (decimal)closed / total * 100 : 0;
        var dormancyRate = total > 0 ? (decimal)dormant / total * 100 : 0;

        return new SavingsProductAccountStatistics(
            total, active, pending, dormant, closed, frozen,
            newLast30Days, newLast12Months,
            Math.Round(closureRate, 2),
            Math.Round(dormancyRate, 2));
    }

    private static SavingsProductBalanceMetrics BuildBalanceMetrics(List<SavingsAccount> accounts, decimal minimumForInterest)
    {
        if (accounts.Count == 0)
        {
            return new SavingsProductBalanceMetrics(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        var activeAccounts = accounts.Where(a => a.Status != SavingsAccount.StatusClosed).ToList();

        var totalBalance = activeAccounts.Sum(a => a.Balance);
        var totalDeposits = accounts.Sum(a => a.TotalDeposits);
        var totalWithdrawals = accounts.Sum(a => a.TotalWithdrawals);
        var totalInterest = accounts.Sum(a => a.TotalInterestEarned);

        var balances = activeAccounts.Select(a => a.Balance).OrderBy(b => b).ToList();
        var avgBalance = activeAccounts.Count > 0 ? totalBalance / activeAccounts.Count : 0;
        var medianBalance = GetMedian(balances);
        var minBalance = balances.Count != 0 ? balances.Min() : 0;
        var maxBalance = balances.Count != 0 ? balances.Max() : 0;

        var belowMinimum = activeAccounts.Count(a => a.Balance < minimumForInterest);
        var percentBelowMin = activeAccounts.Count > 0 ? (decimal)belowMinimum / activeAccounts.Count * 100 : 0;

        return new SavingsProductBalanceMetrics(
            totalBalance,
            totalDeposits,
            totalWithdrawals,
            totalInterest,
            Math.Round(avgBalance, 2),
            Math.Round(medianBalance, 2),
            minBalance,
            maxBalance,
            belowMinimum,
            Math.Round(percentBelowMin, 2));
    }

    private static SavingsProductTransactionMetrics BuildTransactionMetrics(
        List<SavingsTransaction> recentTransactions,
        List<SavingsTransaction> allTransactions)
    {
        var totalLast30 = recentTransactions.Count;
        var depositsLast30 = recentTransactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeDeposit)
            .Sum(t => t.Amount);
        var withdrawalsLast30 = recentTransactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal)
            .Sum(t => t.Amount);

        var depositCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeDeposit);
        var withdrawalCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeWithdrawal);
        var interestCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeInterest);
        var feeCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeFee);
        var transferInCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeTransferIn);
        var transferOutCount = allTransactions.Count(t => t.TransactionType == SavingsTransaction.TypeTransferOut);

        var deposits = allTransactions.Where(t => t.TransactionType == SavingsTransaction.TypeDeposit).ToList();
        var withdrawals = allTransactions.Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal).ToList();

        var avgDeposit = deposits.Count != 0 ? deposits.Average(t => t.Amount) : 0;
        var avgWithdrawal = withdrawals.Count != 0 ? withdrawals.Average(t => t.Amount) : 0;

        return new SavingsProductTransactionMetrics(
            totalLast30,
            depositsLast30,
            withdrawalsLast30,
            depositCount,
            withdrawalCount,
            interestCount,
            feeCount,
            transferInCount,
            transferOutCount,
            Math.Round(avgDeposit, 2),
            Math.Round(avgWithdrawal, 2));
    }

    private static SavingsProductInterestMetrics BuildInterestMetrics(
        List<SavingsAccount> accounts,
        List<SavingsTransaction> transactions,
        decimal annualRate)
    {
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var interestThisMonth = transactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeInterest
                && t.TransactionDate.Month == currentMonth
                && t.TransactionDate.Year == currentYear)
            .Sum(t => t.Amount);

        var interestThisYear = transactions
            .Where(t => t.TransactionType == SavingsTransaction.TypeInterest
                && t.TransactionDate.Year == currentYear)
            .Sum(t => t.Amount);

        var activeAccounts = accounts.Where(a => a.Status == SavingsAccount.StatusActive).ToList();
        var avgInterest = activeAccounts.Count != 0 ? activeAccounts.Average(a => a.TotalInterestEarned) : 0;

        var lastPosting = accounts
            .Where(a => a.LastInterestPostingDate.HasValue)
            .Select(a => a.LastInterestPostingDate)
            .OrderByDescending(d => d)
            .FirstOrDefault();

        // Accounts due for posting (no posting in current period)
        var oneMonthAgoDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1));
        var accountsDue = activeAccounts.Count(a =>
            !a.LastInterestPostingDate.HasValue ||
            a.LastInterestPostingDate.Value < oneMonthAgoDate);

        // Project monthly interest (simplified calculation)
        var totalActiveBalance = activeAccounts.Sum(a => a.Balance);
        var monthlyRate = annualRate / 12 / 100;
        var projectedMonthly = totalActiveBalance * monthlyRate;

        return new SavingsProductInterestMetrics(
            interestThisMonth,
            interestThisYear,
            Math.Round(avgInterest, 2),
            accountsDue,
            lastPosting,
            Math.Round(projectedMonthly, 2));
    }

    private static List<SavingsProductAccountStatusDistribution> BuildStatusDistribution(List<SavingsAccount> accounts)
    {
        var total = accounts.Count;
        if (total == 0) return [];

        var statusGroups = accounts
            .GroupBy(a => a.Status)
            .Select(g => new SavingsProductAccountStatusDistribution(
                g.Key,
                g.Count(),
                g.Sum(a => a.Balance),
                Math.Round((decimal)g.Count() / total * 100, 2)))
            .OrderByDescending(s => s.Count)
            .ToList();

        return statusGroups;
    }

    private static List<SavingsProductMonthlyActivityTrend> BuildMonthlyTrends(
        List<SavingsAccount> accounts,
        List<SavingsTransaction> transactions)
    {
        var trends = new List<SavingsProductMonthlyActivityTrend>();

        for (int i = 11; i >= 0; i--)
        {
            var date = DateTime.UtcNow.AddMonths(-i);
            var month = date.Month;
            var year = date.Year;

            var newAccounts = accounts.Count(a =>
                a.OpenedDate.Month == month && a.OpenedDate.Year == year);

            var closedAccounts = accounts.Count(a =>
                a.ClosedDate.HasValue &&
                a.ClosedDate.Value.Month == month &&
                a.ClosedDate.Value.Year == year);

            var monthlyTransactions = transactions
                .Where(t => t.TransactionDate.Month == month && t.TransactionDate.Year == year)
                .ToList();

            var totalDeposits = monthlyTransactions
                .Where(t => t.TransactionType == SavingsTransaction.TypeDeposit)
                .Sum(t => t.Amount);

            var totalWithdrawals = monthlyTransactions
                .Where(t => t.TransactionType == SavingsTransaction.TypeWithdrawal)
                .Sum(t => t.Amount);

            var interestPaid = monthlyTransactions
                .Where(t => t.TransactionType == SavingsTransaction.TypeInterest)
                .Sum(t => t.Amount);

            trends.Add(new SavingsProductMonthlyActivityTrend(
                date.ToString("MMM"),
                year,
                newAccounts,
                closedAccounts,
                totalDeposits,
                totalWithdrawals,
                totalDeposits - totalWithdrawals,
                interestPaid,
                monthlyTransactions.Count));
        }

        return trends;
    }

    private static List<RecentAccountActivity> BuildRecentAccounts(
        List<SavingsAccount> accounts,
        Dictionary<Guid, string> members)
    {
        return accounts
            .Where(a => a.Status != SavingsAccount.StatusClosed)
            .OrderByDescending(a => a.OpenedDate)
            .Take(10)
            .Select(a => new RecentAccountActivity(
                a.Id,
                a.AccountNumber,
                members.GetValueOrDefault(a.MemberId, "Unknown"),
                a.Balance,
                a.Status,
                a.OpenedDate,
                null, // LastTransactionDate would require additional query
                a.TotalInterestEarned))
            .ToList();
    }

    private static SavingsProductAlerts BuildAlerts(List<SavingsAccount> accounts, SavingsProduct product)
    {
        var dormantCount = accounts.Count(a => a.Status == SavingsAccount.StatusDormant);
        var belowMinCount = accounts.Count(a =>
            a.Status == SavingsAccount.StatusActive &&
            a.Balance < product.MinBalanceForInterest);
        var frozenCount = accounts.Count(a => a.Status == SavingsAccount.StatusFrozen);
        var pendingCount = accounts.Count(a => a.Status == SavingsAccount.StatusPending);

        var alertList = new List<SavingsProductAlert>();

        if (dormantCount > 0)
        {
            alertList.Add(new SavingsProductAlert(
                "DormantAccounts",
                dormantCount > 10 ? "Warning" : "Info",
                $"{dormantCount} accounts have been dormant for extended period",
                dormantCount,
                DateTime.UtcNow));
        }

        if (belowMinCount > 0)
        {
            alertList.Add(new SavingsProductAlert(
                "BelowMinimumBalance",
                belowMinCount > 20 ? "Warning" : "Info",
                $"{belowMinCount} accounts are below minimum balance for interest",
                belowMinCount,
                DateTime.UtcNow));
        }

        if (frozenCount > 0)
        {
            alertList.Add(new SavingsProductAlert(
                "FrozenAccounts",
                "Warning",
                $"{frozenCount} accounts are frozen and require review",
                frozenCount,
                DateTime.UtcNow));
        }

        if (pendingCount > 0)
        {
            alertList.Add(new SavingsProductAlert(
                "PendingActivation",
                pendingCount > 5 ? "Warning" : "Info",
                $"{pendingCount} accounts pending activation",
                pendingCount,
                DateTime.UtcNow));
        }

        if (!product.IsActive)
        {
            alertList.Add(new SavingsProductAlert(
                "ProductInactive",
                "Critical",
                "This savings product is currently inactive",
                0,
                DateTime.UtcNow));
        }

        return new SavingsProductAlerts(
            dormantCount,
            belowMinCount,
            frozenCount,
            pendingCount,
            alertList);
    }

    private static decimal GetMedian(List<decimal> values)
    {
        if (values.Count == 0) return 0;

        var sorted = values.OrderBy(v => v).ToList();
        int mid = sorted.Count / 2;

        if (sorted.Count % 2 == 0)
        {
            return (sorted[mid - 1] + sorted[mid]) / 2;
        }

        return sorted[mid];
    }
}

// Specification classes
public sealed class SavingsAccountsByProductSpec : Specification<SavingsAccount>
{
    public SavingsAccountsByProductSpec(Guid productId)
    {
        Query.Where(a => a.SavingsProductId == productId);
    }
}

public sealed class SavingsTransactionsByAccountIdsSpec : Specification<SavingsTransaction>
{
    public SavingsTransactionsByAccountIdsSpec(List<Guid> accountIds, DateOnly fromDate)
    {
        Query.Where(t => accountIds.Contains(t.SavingsAccountId) && t.TransactionDate >= fromDate);
    }
}

public sealed class MembersByIdsForSavingsSpec : Specification<Member>
{
    public MembersByIdsForSavingsSpec(List<Guid> memberIds)
    {
        Query.Where(m => memberIds.Contains(m.Id));
    }
}
