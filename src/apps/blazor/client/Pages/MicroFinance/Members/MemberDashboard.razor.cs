namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Members;

public partial class MemberDashboard
{
    [Parameter]
    public Guid Id { get; set; }
    private bool _loading;
    private string _memberName = string.Empty;
    private MemberDashboardData _dashboard = new();
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
            new("Members", "/microfinance/members"),
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
            var response = await Client.GetMemberDashboardAsync("1", Id);
            
            // Check if API returned valid data
            if (response != null && !string.IsNullOrEmpty(response.MemberName))
            {
                _dashboard = response.Adapt<MemberDashboardData>();
                _memberName = _dashboard.MemberName;
            }
            else
            {
                // Use mock data if API returns empty
                _dashboard = GenerateMockData();
                _memberName = _dashboard.MemberName;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load dashboard: {ex.Message}. Using sample data.", Severity.Warning);
            _dashboard = GenerateMockData();
            _memberName = _dashboard.MemberName;
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
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        await DialogService.ShowAsync<MemberDashboardHelpDialog>("Member Dashboard Help", options);
    }

    private MudBlazor.Color GetStatusColor(bool isActive)
    {
        return isActive ? MudBlazor.Color.Success : MudBlazor.Color.Error;
    }

    private MudBlazor.Color GetCreditScoreColor(decimal score)
    {
        return score switch
        {
            >= 90 => MudBlazor.Color.Success,
            >= 70 => MudBlazor.Color.Info,
            >= 50 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private MudBlazor.Color GetRiskCategoryColor(string riskCategory)
    {
        return riskCategory switch
        {
            "Low Risk" => MudBlazor.Color.Success,
            "Medium Risk" => MudBlazor.Color.Warning,
            "High Risk" => MudBlazor.Color.Error,
            _ => MudBlazor.Color.Error
        };
    }

    private MudBlazor.Color GetLoanStatusColor(int daysOverdue)
    {
        return daysOverdue switch
        {
            0 => MudBlazor.Color.Success,
            <= 30 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private MudBlazor.Color GetOnTimeColor(decimal percentage)
    {
        return percentage switch
        {
            >= 90 => MudBlazor.Color.Success,
            >= 70 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private MudBlazor.Color GetDueDaysColor(int days)
    {
        return days switch
        {
            <= 7 => MudBlazor.Color.Error,
            <= 14 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Info
        };
    }

    private MudBlazor.Color GetDTIColor(decimal dti)
    {
        return dti switch
        {
            <= 30 => MudBlazor.Color.Success,
            <= 50 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private MemberDashboardData GenerateMockData()
    {
        return new MemberDashboardData
        {
            MemberId = Id,
            MemberNumber = "MEM-2024-001234",
            MemberName = "John Doe",
            MemberSince = new DateOnly(2020, 3, 15),
            IsActive = true,
            Overview = new MemberOverviewData
            {
                TotalNetWorth = 125000m,
                TotalAssets = 175000m,
                TotalLiabilities = 50000m,
                TotalProducts = 5,
                ActiveLoans = 1,
                ActiveSavingsAccounts = 2,
                TotalShares = 500,
                ActiveFixedDeposits = 1
            },
            LoanPortfolio = new LoanPortfolioData
            {
                TotalLoans = 3,
                ActiveLoans = 1,
                CompletedLoans = 2,
                TotalBorrowed = 200000m,
                TotalOutstanding = 50000m,
                TotalPrincipalOutstanding = 45000m,
                TotalInterestOutstanding = 5000m,
                TotalRepaid = 150000m,
                AverageInterestRate = 18.5m,
                ActiveLoanDetails = new List<LoanDetailData>
                {
                    new LoanDetailData
                    {
                        LoanId = Guid.NewGuid(),
                        LoanNumber = "LN-2024-00567",
                        ProductName = "Business Loan",
                        PrincipalAmount = 75000m,
                        OutstandingPrincipal = 45000m,
                        OutstandingInterest = 5000m,
                        TotalOutstanding = 50000m,
                        InterestRate = 18.5m,
                        DisbursementDate = new DateOnly(2024, 1, 15),
                        ExpectedEndDate = new DateOnly(2025, 1, 15),
                        DaysUntilNextPayment = 12,
                        DaysOverdue = 0,
                        Status = "DISBURSED"
                    }
                }
            },
            SavingsPortfolio = new SavingsPortfolioData
            {
                TotalAccounts = 2,
                ActiveAccounts = 2,
                DormantAccounts = 0,
                TotalBalance = 75000m,
                TotalDeposits = 85000m,
                TotalWithdrawals = 12000m,
                TotalInterestEarned = 2000m,
                AverageBalance = 37500m,
                AccountDetails = new List<SavingsAccountDetailData>
                {
                    new SavingsAccountDetailData
                    {
                        AccountId = Guid.NewGuid(),
                        AccountNumber = "SAV-2024-00123",
                        ProductName = "Regular Savings",
                        CurrentBalance = 50000m,
                        InterestRate = 4.5m,
                        InterestEarned = 1500m,
                        OpenedDate = new DateOnly(2020, 3, 15),
                        LastTransactionDate = new DateOnly(2024, 11, 10),
                        Status = "Active"
                    },
                    new SavingsAccountDetailData
                    {
                        AccountId = Guid.NewGuid(),
                        AccountNumber = "SAV-2024-00456",
                        ProductName = "Emergency Fund",
                        CurrentBalance = 25000m,
                        InterestRate = 5.0m,
                        InterestEarned = 500m,
                        OpenedDate = new DateOnly(2022, 6, 1),
                        LastTransactionDate = new DateOnly(2024, 10, 25),
                        Status = "Active"
                    }
                }
            },
            SharePortfolio = new SharePortfolioData
            {
                TotalShareAccounts = 1,
                TotalShares = 500,
                TotalShareValue = 50000m,
                TotalDividendsEarned = 2500m,
                CurrentSharePrice = 100m,
                AccountDetails = new List<ShareAccountDetailData>
                {
                    new ShareAccountDetailData
                    {
                        AccountId = Guid.NewGuid(),
                        AccountNumber = "SHR-2020-00789",
                        NumberOfShares = 500,
                        ShareValue = 100m,
                        TotalValue = 50000m,
                        PurchaseDate = new DateOnly(2020, 3, 15),
                        Status = "Active"
                    }
                }
            },
            FixedDeposits = new FixedDepositSummaryData
            {
                TotalDeposits = 2,
                ActiveDeposits = 1,
                MaturedDeposits = 1,
                TotalPrincipal = 50000m,
                TotalInterestEarned = 3000m,
                TotalMaturityValue = 53000m,
                DepositDetails = new List<FixedDepositDetailData>
                {
                    new FixedDepositDetailData
                    {
                        DepositId = Guid.NewGuid(),
                        CertificateNumber = "FD-2024-00234",
                        PrincipalAmount = 50000m,
                        InterestRate = 8.0m,
                        AccruedInterest = 3000m,
                        MaturityAmount = 53000m,
                        StartDate = new DateOnly(2024, 1, 1),
                        MaturityDate = new DateOnly(2025, 1, 1),
                        DaysToMaturity = 45,
                        Status = "Active"
                    }
                }
            },
            Fees = new FeesSummaryData
            {
                TotalFeesCharged = 5000m,
                TotalFeesPaid = 4500m,
                TotalFeesOutstanding = 500m,
                TotalFeeTransactions = 12,
                FeesByType = new List<FeeBreakdownData>
                {
                    new FeeBreakdownData { FeeType = "Processing Fee", TotalAmount = 2000m, Count = 3 },
                    new FeeBreakdownData { FeeType = "Monthly Fee", TotalAmount = 2400m, Count = 8 },
                    new FeeBreakdownData { FeeType = "Late Fee", TotalAmount = 600m, Count = 1 }
                }
            },
            RepaymentPerformance = new RepaymentPerformanceData
            {
                TotalPaymentsMade = 24,
                OnTimePayments = 22,
                LatePayments = 2,
                OnTimePaymentPercentage = 91.7m,
                TotalPrincipalRepaid = 130000m,
                TotalInterestRepaid = 20000m,
                CurrentDaysOverdue = 0,
                MaxDaysOverdue = 15,
                Last12MonthsPerformance = new List<MonthlyRepaymentData>()
            },
            RecentTransactions = new List<RecentTransactionData>
            {
                new RecentTransactionData
                {
                    TransactionId = Guid.NewGuid(),
                    TransactionDate = new DateOnly(2024, 11, 10),
                    TransactionType = "Loan Repayment",
                    Description = "Monthly payment",
                    AccountNumber = "LN-2024-00567",
                    Amount = 5500m,
                    Direction = "Debit"
                },
                new RecentTransactionData
                {
                    TransactionId = Guid.NewGuid(),
                    TransactionDate = new DateOnly(2024, 11, 5),
                    TransactionType = "Deposit",
                    Description = "Salary deposit",
                    AccountNumber = "SAV-2024-00123",
                    Amount = 15000m,
                    Direction = "Credit"
                }
            },
            UpcomingPayments = new List<UpcomingPaymentData>
            {
                new UpcomingPaymentData
                {
                    DueDate = new DateOnly(2024, 12, 10),
                    LoanNumber = "LN-2024-00567",
                    PrincipalDue = 4500m,
                    InterestDue = 1000m,
                    TotalDue = 5500m,
                    DaysUntilDue = 12
                }
            },
            ProductHoldings = new List<ProductHoldingData>
            {
                new ProductHoldingData { ProductType = "Loan", ProductName = "Business Loan", Count = 1, TotalValue = 50000m, Status = "Active" },
                new ProductHoldingData { ProductType = "Savings", ProductName = "Regular Savings", Count = 2, TotalValue = 75000m, Status = "Active" },
                new ProductHoldingData { ProductType = "Shares", ProductName = "Share Capital", Count = 500, TotalValue = 50000m, Status = "Active" },
                new ProductHoldingData { ProductType = "Fixed Deposit", ProductName = "Term Deposit", Count = 1, TotalValue = 50000m, Status = "Active" }
            },
            CreditIndicators = new CreditIndicatorsData
            {
                RepaymentScore = 92m,
                RiskCategory = "Low Risk",
                MembershipTenureDays = 1700,
                TotalLifetimeBorrowing = 200000m,
                TotalLifetimeRepaid = 150000m,
                TotalLoansCompleted = 2,
                TotalDefaultedLoans = 0,
                DebtToIncomeRatio = 25m,
                IsEligibleForNewLoan = true,
                MaxEligibleLoanAmount = 150000m
            }
        };
    }

    // Local DTO classes for mock data
    public class MemberDashboardData
    {
        public Guid MemberId { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public DateOnly MemberSince { get; set; }
        public bool IsActive { get; set; }
        public MemberOverviewData Overview { get; set; } = new();
        public LoanPortfolioData LoanPortfolio { get; set; } = new();
        public SavingsPortfolioData SavingsPortfolio { get; set; } = new();
        public SharePortfolioData SharePortfolio { get; set; } = new();
        public FixedDepositSummaryData FixedDeposits { get; set; } = new();
        public FeesSummaryData Fees { get; set; } = new();
        public RepaymentPerformanceData RepaymentPerformance { get; set; } = new();
        public List<RecentTransactionData> RecentTransactions { get; set; } = new();
        public List<UpcomingPaymentData> UpcomingPayments { get; set; } = new();
        public List<ProductHoldingData> ProductHoldings { get; set; } = new();
        public CreditIndicatorsData CreditIndicators { get; set; } = new();
    }

    public class MemberOverviewData
    {
        public decimal TotalNetWorth { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal TotalLiabilities { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveLoans { get; set; }
        public int ActiveSavingsAccounts { get; set; }
        public int TotalShares { get; set; }
        public int ActiveFixedDeposits { get; set; }
    }

    public class LoanPortfolioData
    {
        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }
        public int CompletedLoans { get; set; }
        public decimal TotalBorrowed { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal TotalPrincipalOutstanding { get; set; }
        public decimal TotalInterestOutstanding { get; set; }
        public decimal TotalRepaid { get; set; }
        public decimal AverageInterestRate { get; set; }
        public List<LoanDetailData> ActiveLoanDetails { get; set; } = new();
    }

    public class LoanDetailData
    {
        public Guid LoanId { get; set; }
        public string LoanNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal PrincipalAmount { get; set; }
        public decimal OutstandingPrincipal { get; set; }
        public decimal OutstandingInterest { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal InterestRate { get; set; }
        public DateOnly DisbursementDate { get; set; }
        public DateOnly ExpectedEndDate { get; set; }
        public int DaysUntilNextPayment { get; set; }
        public int DaysOverdue { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class SavingsPortfolioData
    {
        public int TotalAccounts { get; set; }
        public int ActiveAccounts { get; set; }
        public int DormantAccounts { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal AverageBalance { get; set; }
        public List<SavingsAccountDetailData> AccountDetails { get; set; } = new();
    }

    public class SavingsAccountDetailData
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestEarned { get; set; }
        public DateOnly OpenedDate { get; set; }
        public DateOnly? LastTransactionDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class SharePortfolioData
    {
        public int TotalShareAccounts { get; set; }
        public int TotalShares { get; set; }
        public decimal TotalShareValue { get; set; }
        public decimal TotalDividendsEarned { get; set; }
        public decimal CurrentSharePrice { get; set; }
        public List<ShareAccountDetailData> AccountDetails { get; set; } = new();
    }

    public class ShareAccountDetailData
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public int NumberOfShares { get; set; }
        public decimal ShareValue { get; set; }
        public decimal TotalValue { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class FixedDepositSummaryData
    {
        public int TotalDeposits { get; set; }
        public int ActiveDeposits { get; set; }
        public int MaturedDeposits { get; set; }
        public decimal TotalPrincipal { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public decimal TotalMaturityValue { get; set; }
        public List<FixedDepositDetailData> DepositDetails { get; set; } = new();
    }

    public class FixedDepositDetailData
    {
        public Guid DepositId { get; set; }
        public string CertificateNumber { get; set; } = string.Empty;
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal AccruedInterest { get; set; }
        public decimal MaturityAmount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly MaturityDate { get; set; }
        public int DaysToMaturity { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class FeesSummaryData
    {
        public decimal TotalFeesCharged { get; set; }
        public decimal TotalFeesPaid { get; set; }
        public decimal TotalFeesOutstanding { get; set; }
        public int TotalFeeTransactions { get; set; }
        public List<FeeBreakdownData> FeesByType { get; set; } = new();
    }

    public class FeeBreakdownData
    {
        public string FeeType { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int Count { get; set; }
    }

    public class RepaymentPerformanceData
    {
        public int TotalPaymentsMade { get; set; }
        public int OnTimePayments { get; set; }
        public int LatePayments { get; set; }
        public decimal OnTimePaymentPercentage { get; set; }
        public decimal TotalPrincipalRepaid { get; set; }
        public decimal TotalInterestRepaid { get; set; }
        public int CurrentDaysOverdue { get; set; }
        public int MaxDaysOverdue { get; set; }
        public List<MonthlyRepaymentData> Last12MonthsPerformance { get; set; } = new();
    }

    public class MonthlyRepaymentData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal ExpectedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public bool OnTime { get; set; }
    }

    public class RecentTransactionData
    {
        public Guid TransactionId { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Direction { get; set; } = string.Empty;
    }

    public class UpcomingPaymentData
    {
        public DateOnly DueDate { get; set; }
        public string LoanNumber { get; set; } = string.Empty;
        public decimal PrincipalDue { get; set; }
        public decimal InterestDue { get; set; }
        public decimal TotalDue { get; set; }
        public int DaysUntilDue { get; set; }
    }

    public class ProductHoldingData
    {
        public string ProductType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreditIndicatorsData
    {
        public decimal RepaymentScore { get; set; }
        public string RiskCategory { get; set; } = string.Empty;
        public int MembershipTenureDays { get; set; }
        public decimal TotalLifetimeBorrowing { get; set; }
        public decimal TotalLifetimeRepaid { get; set; }
        public int TotalLoansCompleted { get; set; }
        public int TotalDefaultedLoans { get; set; }
        public decimal DebtToIncomeRatio { get; set; }
        public bool IsEligibleForNewLoan { get; set; }
        public decimal MaxEligibleLoanAmount { get; set; }
    }
}
