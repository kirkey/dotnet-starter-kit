using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.LoanProducts;

public partial class LoanProductDashboard
{
    [Parameter]
    public Guid Id { get; set; }

    private bool _loading;
    private string _productName = string.Empty;
    private LoanProductDashboardData _dashboard = new();
    private List<BreadcrumbItem> _breadcrumbs = new();

    protected override async Task OnInitializedAsync()
    {
        _breadcrumbs = new List<BreadcrumbItem>
        {
            new("Home", "/", icon: Icons.Material.Filled.Home),
            new("MicroFinance", "/microfinance"),
            new("Loan Products", "/microfinance/loan-products"),
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
            var response = await Client.GetLoanProductDashboardAsync("1", Id);
            
            if (response != null && !string.IsNullOrEmpty(response.ProductName))
            {
                _dashboard = response.Adapt<LoanProductDashboardData>();
                _productName = _dashboard.ProductName;
            }
            else
            {
                _dashboard = GenerateMockData();
                _productName = _dashboard.ProductName;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            _dashboard = GenerateMockData();
            _productName = _dashboard.ProductName;
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
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        await DialogService.ShowAsync<LoanProductDashboardHelpDialog>("Loan Product Dashboard Help", options);
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

    private static MudBlazor.Color GetCollectionRateColor(decimal rate)
    {
        return rate switch
        {
            >= 95 => MudBlazor.Color.Success,
            >= 80 => MudBlazor.Color.Info,
            >= 60 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private static MudBlazor.Color GetPARColor(decimal par)
    {
        return par switch
        {
            <= 2 => MudBlazor.Color.Success,
            <= 5 => MudBlazor.Color.Info,
            <= 10 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private static MudBlazor.Color GetOnTimeColor(decimal rate)
    {
        return rate switch
        {
            >= 90 => MudBlazor.Color.Success,
            >= 75 => MudBlazor.Color.Info,
            >= 60 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private static MudBlazor.Color GetWriteOffRateColor(decimal rate)
    {
        return rate switch
        {
            <= 1 => MudBlazor.Color.Success,
            <= 3 => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private static MudBlazor.Color GetStatusColor(string status)
    {
        return status.ToUpperInvariant() switch
        {
            "PENDING" => MudBlazor.Color.Warning,
            "APPROVED" => MudBlazor.Color.Info,
            "DISBURSED" => MudBlazor.Color.Success,
            "CLOSED" => MudBlazor.Color.Default,
            "REJECTED" => MudBlazor.Color.Error,
            "WRITTEN_OFF" => MudBlazor.Color.Dark,
            _ => MudBlazor.Color.Default
        };
    }

    private LoanProductDashboardData GenerateMockData()
    {
        var random = new Random();
        return new LoanProductDashboardData
        {
            LoanProductId = Id,
            ProductCode = "AGRI-001",
            ProductName = "Agricultural Loan",
            IsActive = true,
            Overview = new LoanProductOverviewData
            {
                TotalLoansIssued = random.Next(150, 300),
                ActiveLoans = random.Next(80, 150),
                PendingLoans = random.Next(5, 20),
                ApprovedLoans = random.Next(2, 10),
                ClosedLoans = random.Next(40, 80),
                RejectedLoans = random.Next(5, 15),
                WrittenOffLoans = random.Next(1, 5),
                TotalAmountDisbursed = random.Next(50000000, 100000000),
                TotalOutstanding = random.Next(20000000, 50000000),
                AverageInterestRate = 14.5m + (decimal)(random.NextDouble() * 2),
                AverageLoanSize = random.Next(200000, 500000),
                AverageTermMonths = random.Next(6, 18),
                ProductInterestRate = 15.0m,
                MinLoanAmount = 50000,
                MaxLoanAmount = 2000000
            },
            Portfolio = new LoanPortfolioMetricsData
            {
                TotalPrincipalDisbursed = random.Next(45000000, 90000000),
                TotalPrincipalOutstanding = random.Next(18000000, 45000000),
                TotalPrincipalCollected = random.Next(20000000, 45000000),
                TotalInterestExpected = random.Next(8000000, 15000000),
                TotalInterestOutstanding = random.Next(2000000, 5000000),
                TotalInterestCollected = random.Next(5000000, 10000000),
                MinLoanIssued = 50000,
                MaxLoanIssued = 1800000,
                ShortestTermMonths = 3,
                LongestTermMonths = 24,
                TotalBorrowers = random.Next(100, 200)
            },
            Repayments = new RepaymentMetricsData
            {
                TotalCollected = random.Next(30000000, 60000000),
                TotalPrincipalCollected = random.Next(20000000, 45000000),
                TotalInterestCollected = random.Next(5000000, 10000000),
                TotalPenaltiesCollected = random.Next(100000, 500000),
                TotalRepayments = random.Next(500, 1500),
                CollectionRate = 85 + (decimal)(random.NextDouble() * 12),
                OnTimeRepaymentRate = 75 + (decimal)(random.NextDouble() * 20),
                OnTimeRepayments = random.Next(400, 1200),
                LateRepayments = random.Next(50, 200),
                AverageRepaymentAmount = random.Next(30000, 80000)
            },
            Delinquency = new DelinquencyMetricsData
            {
                OverdueLoans = random.Next(5, 20),
                OverdueAmount = random.Next(1000000, 5000000),
                PortfolioAtRisk1Day = 2 + (decimal)(random.NextDouble() * 5),
                PortfolioAtRisk30Days = 1 + (decimal)(random.NextDouble() * 4),
                PortfolioAtRisk60Days = 0.5m + (decimal)(random.NextDouble() * 2),
                PortfolioAtRisk90Days = (decimal)(random.NextDouble() * 1.5),
                WrittenOffCount = random.Next(1, 5),
                WrittenOffAmount = random.Next(500000, 2000000),
                WriteOffRate = (decimal)(random.NextDouble() * 3),
                LoansInArrears = random.Next(8, 25),
                ArrearsAmount = random.Next(2000000, 6000000)
            },
            LoansByStatus = new List<LoanDistributionData>
            {
                new() { Status = "DISBURSED", Count = random.Next(80, 150), TotalAmount = random.Next(30000000, 60000000), Percentage = 55 },
                new() { Status = "CLOSED", Count = random.Next(40, 80), TotalAmount = random.Next(15000000, 30000000), Percentage = 25 },
                new() { Status = "PENDING", Count = random.Next(5, 20), TotalAmount = random.Next(2000000, 8000000), Percentage = 8 },
                new() { Status = "APPROVED", Count = random.Next(2, 10), TotalAmount = random.Next(1000000, 4000000), Percentage = 4 },
                new() { Status = "REJECTED", Count = random.Next(5, 15), TotalAmount = random.Next(2000000, 6000000), Percentage = 5 },
                new() { Status = "WRITTEN_OFF", Count = random.Next(1, 5), TotalAmount = random.Next(500000, 2000000), Percentage = 3 }
            },
            RecentActivity = Enumerable.Range(1, 10).Select(i => new RecentLoanActivityData
            {
                LoanId = Guid.NewGuid(),
                LoanNumber = $"LN-{DateTime.Now.Year}-{random.Next(1000, 9999)}",
                MemberName = $"Member {random.Next(100, 999)}",
                PrincipalAmount = random.Next(100000, 800000),
                DisbursementDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-random.Next(1, 90))),
                Status = new[] { "DISBURSED", "DISBURSED", "DISBURSED", "CLOSED", "PENDING" }[random.Next(5)],
                OutstandingBalance = random.Next(0, 600000)
            }).OrderByDescending(x => x.DisbursementDate).ToList(),
            Alerts = new LoanProductAlertsData
            {
                Items = new List<LoanProductAlertData>
                {
                    new() { Severity = "Warning", Title = "High PAR 30", Description = "PAR 30 is at 4.2%. Monitor collection efforts.", ActionUrl = "/microfinance/loans?filter=overdue" },
                    new() { Severity = "Info", Title = "Pending Applications", Description = "12 loan applications awaiting approval.", ActionUrl = "/microfinance/loans?filter=pending" }
                },
                TotalAlerts = 2,
                CriticalCount = 0,
                WarningCount = 1,
                InfoCount = 1
            }
        };
    }

    // Data classes
    private class LoanProductDashboardData
    {
        public Guid LoanProductId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public LoanProductOverviewData Overview { get; set; } = new();
        public LoanPortfolioMetricsData Portfolio { get; set; } = new();
        public RepaymentMetricsData Repayments { get; set; } = new();
        public DelinquencyMetricsData Delinquency { get; set; } = new();
        public List<LoanDistributionData> LoansByStatus { get; set; } = new();
        public List<RecentLoanActivityData> RecentActivity { get; set; } = new();
        public LoanProductAlertsData Alerts { get; set; } = new();
    }

    private class LoanProductOverviewData
    {
        public int TotalLoansIssued { get; set; }
        public int ActiveLoans { get; set; }
        public int PendingLoans { get; set; }
        public int ApprovedLoans { get; set; }
        public int ClosedLoans { get; set; }
        public int RejectedLoans { get; set; }
        public int WrittenOffLoans { get; set; }
        public decimal TotalAmountDisbursed { get; set; }
        public decimal TotalOutstanding { get; set; }
        public decimal AverageInterestRate { get; set; }
        public decimal AverageLoanSize { get; set; }
        public int AverageTermMonths { get; set; }
        public decimal ProductInterestRate { get; set; }
        public decimal MinLoanAmount { get; set; }
        public decimal MaxLoanAmount { get; set; }
    }

    private class LoanPortfolioMetricsData
    {
        public decimal TotalPrincipalDisbursed { get; set; }
        public decimal TotalPrincipalOutstanding { get; set; }
        public decimal TotalPrincipalCollected { get; set; }
        public decimal TotalInterestExpected { get; set; }
        public decimal TotalInterestOutstanding { get; set; }
        public decimal TotalInterestCollected { get; set; }
        public decimal MinLoanIssued { get; set; }
        public decimal MaxLoanIssued { get; set; }
        public int ShortestTermMonths { get; set; }
        public int LongestTermMonths { get; set; }
        public int TotalBorrowers { get; set; }
    }

    private class RepaymentMetricsData
    {
        public decimal TotalCollected { get; set; }
        public decimal TotalPrincipalCollected { get; set; }
        public decimal TotalInterestCollected { get; set; }
        public decimal TotalPenaltiesCollected { get; set; }
        public int TotalRepayments { get; set; }
        public decimal CollectionRate { get; set; }
        public decimal OnTimeRepaymentRate { get; set; }
        public int OnTimeRepayments { get; set; }
        public int LateRepayments { get; set; }
        public decimal AverageRepaymentAmount { get; set; }
    }

    private class DelinquencyMetricsData
    {
        public int OverdueLoans { get; set; }
        public decimal OverdueAmount { get; set; }
        public decimal PortfolioAtRisk1Day { get; set; }
        public decimal PortfolioAtRisk30Days { get; set; }
        public decimal PortfolioAtRisk60Days { get; set; }
        public decimal PortfolioAtRisk90Days { get; set; }
        public int WrittenOffCount { get; set; }
        public decimal WrittenOffAmount { get; set; }
        public decimal WriteOffRate { get; set; }
        public int LoansInArrears { get; set; }
        public decimal ArrearsAmount { get; set; }
    }

    private class LoanDistributionData
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Percentage { get; set; }
    }

    private class RecentLoanActivityData
    {
        public Guid LoanId { get; set; }
        public string LoanNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public decimal PrincipalAmount { get; set; }
        public DateOnly? DisbursementDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal OutstandingBalance { get; set; }
    }

    private class LoanProductAlertsData
    {
        public List<LoanProductAlertData> Items { get; set; } = new();
        public int TotalAlerts { get; set; }
        public int CriticalCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
    }

    private class LoanProductAlertData
    {
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
    }
}
