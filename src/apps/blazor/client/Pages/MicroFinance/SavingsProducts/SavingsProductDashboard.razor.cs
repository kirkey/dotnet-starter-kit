namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.SavingsProducts;

public partial class SavingsProductDashboard
{
    [Parameter]
    public Guid Id { get; set; }

    private bool _loading;
    private string _productName = string.Empty;
    private SavingsProductDashboardData _dashboard = new();
    private List<BreadcrumbItem> _breadcrumbs = new();
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        _breadcrumbs = new List<BreadcrumbItem>
        {
            new("Home", "/", icon: Icons.Material.Filled.Home),
            new("MicroFinance", "/microfinance"),
            new("Savings Products", "/microfinance/savings-products"),
            new("Dashboard", null, disabled: true)
        };

        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            var response = await Client.GetSavingsProductDashboardAsync("1", Id);
            
            if (response != null && response.Overview != null && !string.IsNullOrEmpty(response.Overview.Name))
            {
                _dashboard = response.Adapt<SavingsProductDashboardData>();
                _productName = _dashboard.Overview.Name;
            }
            else
            {
                _dashboard = GenerateMockData();
                _productName = _dashboard.Overview.Name;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            _dashboard = GenerateMockData();
            _productName = _dashboard.Overview.Name;
            Snackbar.Add($"Error loading dashboard. Using sample data: {ex.Message}", Severity.Warning);
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task RefreshData()
    {
        await LoadDashboardData();
    }

    private async Task OpenHelpDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        await DialogService.ShowAsync<SavingsProductDashboardHelpDialog>("Savings Product Dashboard Help", options);
    }

    private static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C0");
    }

    private static Severity GetAlertSeverity(string severity)
    {
        return severity switch
        {
            "Critical" => Severity.Error,
            "Warning" => Severity.Warning,
            "Info" => Severity.Info,
            _ => Severity.Normal
        };
    }

    private static MudBlazor.Color GetStatusColor(string status)
    {
        return status switch
        {
            "Active" => MudBlazor.Color.Success,
            "Pending" => MudBlazor.Color.Warning,
            "Dormant" => MudBlazor.Color.Info,
            "Frozen" => MudBlazor.Color.Error,
            "Closed" => MudBlazor.Color.Default,
            _ => MudBlazor.Color.Default
        };
    }

    private static MudBlazor.Color GetBelowMinColor(decimal percentage)
    {
        return percentage switch
        {
            <= 10 => MudBlazor.Color.Success,
            <= 25 => MudBlazor.Color.Info,
            <= 50 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private static MudBlazor.Color GetDuePostingColor(int count)
    {
        return count switch
        {
            0 => MudBlazor.Color.Success,
            <= 10 => MudBlazor.Color.Info,
            <= 50 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private SavingsProductDashboardData GenerateMockData()
    {
        return new SavingsProductDashboardData
        {
            ProductCode = "SAV-REG-001",
            IsActive = true,
            Overview = new SavingsProductOverviewData
            {
                ProductId = Id,
                Code = "SAV-REG-001",
                Name = "Regular Savings Account",
                Description = "Standard savings account with competitive interest rates",
                InterestRate = 4.5m,
                InterestCalculation = "Daily",
                InterestPostingFrequency = "Monthly",
                MinOpeningBalance = 500m,
                MinBalanceForInterest = 1000m,
                MinWithdrawalAmount = 100m,
                MaxWithdrawalPerDay = 50000m,
                AllowOverdraft = false,
                OverdraftLimit = null,
                IsActive = true
            },
            AccountStats = new AccountStatisticsData
            {
                TotalAccounts = 1250,
                ActiveAccounts = 980,
                PendingAccounts = 45,
                DormantAccounts = 180,
                ClosedAccounts = 40,
                FrozenAccounts = 5,
                NewAccountsLast30Days = 35,
                NewAccountsLast12Months = 420,
                AccountClosureRate = 3.2m,
                DormancyRate = 14.4m
            },
            BalanceMetrics = new BalanceMetricsData
            {
                TotalBalance = 45250000m,
                TotalDeposits = 62500000m,
                TotalWithdrawals = 18750000m,
                TotalInterestEarned = 1500000m,
                AverageBalance = 36200m,
                MedianBalance = 25000m,
                MinBalance = 0m,
                MaxBalance = 2500000m,
                AccountsBelowMinimum = 125,
                PercentageBelowMinimum = 12.8m
            },
            TransactionMetrics = new TransactionMetricsData
            {
                TotalTransactionsLast30Days = 3450,
                TotalDepositAmountLast30Days = 8500000m,
                TotalWithdrawalAmountLast30Days = 4200000m,
                DepositCount = 28500,
                WithdrawalCount = 18200,
                InterestPostingCount = 11760,
                FeeTransactionCount = 450,
                TransferInCount = 2100,
                TransferOutCount = 1850,
                AverageDepositAmount = 2200m,
                AverageWithdrawalAmount = 1850m
            },
            InterestMetrics = new InterestMetricsData
            {
                TotalInterestPaidThisMonth = 125000m,
                TotalInterestPaidThisYear = 1450000m,
                AverageInterestPerAccount = 1531m,
                AccountsDueForPosting = 45,
                LastInterestPostingDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                ProjectedMonthlyInterest = 169688m
            },
            StatusDistribution = new List<AccountStatusDistributionData>
            {
                new() { Status = "Active", Count = 980, TotalBalance = 42500000m, Percentage = 78.4m },
                new() { Status = "Dormant", Count = 180, TotalBalance = 2250000m, Percentage = 14.4m },
                new() { Status = "Pending", Count = 45, TotalBalance = 350000m, Percentage = 3.6m },
                new() { Status = "Closed", Count = 40, TotalBalance = 0m, Percentage = 3.2m },
                new() { Status = "Frozen", Count = 5, TotalBalance = 150000m, Percentage = 0.4m }
            },
            MonthlyTrends = GenerateMonthlyTrends(),
            RecentAccounts = new List<RecentAccountActivityData>
            {
                new() { AccountId = Guid.NewGuid(), AccountNumber = "SAV-2024-001234", MemberName = "John Smith", CurrentBalance = 45000m, Status = "Active", OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)), TotalInterestEarned = 125m },
                new() { AccountId = Guid.NewGuid(), AccountNumber = "SAV-2024-001233", MemberName = "Mary Johnson", CurrentBalance = 125000m, Status = "Active", OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)), TotalInterestEarned = 450m },
                new() { AccountId = Guid.NewGuid(), AccountNumber = "SAV-2024-001232", MemberName = "David Wilson", CurrentBalance = 8500m, Status = "Active", OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), TotalInterestEarned = 35m },
                new() { AccountId = Guid.NewGuid(), AccountNumber = "SAV-2024-001231", MemberName = "Sarah Brown", CurrentBalance = 500m, Status = "Pending", OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-8)), TotalInterestEarned = 0m },
                new() { AccountId = Guid.NewGuid(), AccountNumber = "SAV-2024-001230", MemberName = "Michael Davis", CurrentBalance = 67500m, Status = "Active", OpenedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)), TotalInterestEarned = 285m }
            },
            Alerts = new SavingsProductAlertsData
            {
                DormantAccountsCount = 180,
                AccountsBelowMinimumCount = 125,
                FrozenAccountsCount = 5,
                PendingActivationCount = 45,
                AlertList = new List<SavingsProductAlertData>
                {
                    new() { AlertType = "DormantAccounts", Severity = "Warning", Message = "180 accounts have been dormant for extended period", AffectedCount = 180, CreatedAt = DateTime.UtcNow },
                    new() { AlertType = "BelowMinimumBalance", Severity = "Info", Message = "125 accounts are below minimum balance for interest", AffectedCount = 125, CreatedAt = DateTime.UtcNow },
                    new() { AlertType = "FrozenAccounts", Severity = "Warning", Message = "5 accounts are frozen and require review", AffectedCount = 5, CreatedAt = DateTime.UtcNow }
                }
            }
        };
    }

    private static List<MonthlyActivityTrendData> GenerateMonthlyTrends()
    {
        var trends = new List<MonthlyActivityTrendData>();
        var random = new Random(42);

        for (int i = 11; i >= 0; i--)
        {
            var date = DateTime.UtcNow.AddMonths(-i);
            var deposits = random.Next(5000000, 10000000);
            var withdrawals = random.Next(2500000, 6000000);

            trends.Add(new MonthlyActivityTrendData
            {
                Month = date.ToString("MMM"),
                Year = date.Year,
                NewAccounts = random.Next(25, 50),
                ClosedAccounts = random.Next(2, 8),
                TotalDeposits = deposits,
                TotalWithdrawals = withdrawals,
                NetFlow = deposits - withdrawals,
                InterestPaid = random.Next(100000, 150000),
                TransactionCount = random.Next(2500, 4000)
            });
        }

        return trends;
    }

    // Local data classes for mock data
    private class SavingsProductDashboardData
    {
        public string ProductCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public SavingsProductOverviewData Overview { get; set; } = new();
        public AccountStatisticsData AccountStats { get; set; } = new();
        public BalanceMetricsData BalanceMetrics { get; set; } = new();
        public TransactionMetricsData TransactionMetrics { get; set; } = new();
        public InterestMetricsData InterestMetrics { get; set; } = new();
        public List<AccountStatusDistributionData> StatusDistribution { get; set; } = new();
        public List<MonthlyActivityTrendData> MonthlyTrends { get; set; } = new();
        public List<RecentAccountActivityData> RecentAccounts { get; set; } = new();
        public SavingsProductAlertsData Alerts { get; set; } = new();
    }

    private class SavingsProductOverviewData
    {
        public Guid ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal InterestRate { get; set; }
        public string InterestCalculation { get; set; } = string.Empty;
        public string InterestPostingFrequency { get; set; } = string.Empty;
        public decimal MinOpeningBalance { get; set; }
        public decimal MinBalanceForInterest { get; set; }
        public decimal MinWithdrawalAmount { get; set; }
        public decimal? MaxWithdrawalPerDay { get; set; }
        public bool AllowOverdraft { get; set; }
        public decimal? OverdraftLimit { get; set; }
        public bool IsActive { get; set; }
    }

    private class AccountStatisticsData
    {
        public int TotalAccounts { get; set; }
        public int ActiveAccounts { get; set; }
        public int PendingAccounts { get; set; }
        public int DormantAccounts { get; set; }
        public int ClosedAccounts { get; set; }
        public int FrozenAccounts { get; set; }
        public int NewAccountsLast30Days { get; set; }
        public int NewAccountsLast12Months { get; set; }
        public decimal AccountClosureRate { get; set; }
        public decimal DormancyRate { get; set; }
    }

    private class BalanceMetricsData
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal AverageBalance { get; set; }
        public decimal MedianBalance { get; set; }
        public decimal MinBalance { get; set; }
        public decimal MaxBalance { get; set; }
        public int AccountsBelowMinimum { get; set; }
        public decimal PercentageBelowMinimum { get; set; }
    }

    private class TransactionMetricsData
    {
        public int TotalTransactionsLast30Days { get; set; }
        public decimal TotalDepositAmountLast30Days { get; set; }
        public decimal TotalWithdrawalAmountLast30Days { get; set; }
        public int DepositCount { get; set; }
        public int WithdrawalCount { get; set; }
        public int InterestPostingCount { get; set; }
        public int FeeTransactionCount { get; set; }
        public int TransferInCount { get; set; }
        public int TransferOutCount { get; set; }
        public decimal AverageDepositAmount { get; set; }
        public decimal AverageWithdrawalAmount { get; set; }
    }

    private class InterestMetricsData
    {
        public decimal TotalInterestPaidThisMonth { get; set; }
        public decimal TotalInterestPaidThisYear { get; set; }
        public decimal AverageInterestPerAccount { get; set; }
        public int AccountsDueForPosting { get; set; }
        public DateOnly? LastInterestPostingDate { get; set; }
        public decimal ProjectedMonthlyInterest { get; set; }
    }

    private class AccountStatusDistributionData
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal Percentage { get; set; }
    }

    private class MonthlyActivityTrendData
    {
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public int NewAccounts { get; set; }
        public int ClosedAccounts { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal NetFlow { get; set; }
        public decimal InterestPaid { get; set; }
        public int TransactionCount { get; set; }
    }

    private class RecentAccountActivityData
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly OpenedDate { get; set; }
        public DateOnly? LastTransactionDate { get; set; }
        public decimal TotalInterestEarned { get; set; }
    }

    private class SavingsProductAlertsData
    {
        public int DormantAccountsCount { get; set; }
        public int AccountsBelowMinimumCount { get; set; }
        public int FrozenAccountsCount { get; set; }
        public int PendingActivationCount { get; set; }
        public List<SavingsProductAlertData> AlertList { get; set; } = new();
    }

    private class SavingsProductAlertData
    {
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int AffectedCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
