using FSH.Starter.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Branches;

public partial class BranchDashboard
{
    [Parameter]
    public DefaultIdType Id { get; set; }

    private List<BreadcrumbItem> _breadcrumbs = new();
    private bool _loading = true;
    private string _branchName = "Loading...";
    private BranchDashboardData _dashboard = new();
    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
        
        _breadcrumbs = new List<BreadcrumbItem>
        {
            new("Home", "/", false, Icons.Material.Filled.Home),
            new("MicroFinance", "/microfinance", false, Icons.Material.Filled.AccountBalance),
            new("Branches", "/microfinance/branches", false, Icons.Material.Filled.Business),
            new("Dashboard", null, true, Icons.Material.Filled.Dashboard)
        };

        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _loading = true;
        try
        {
            var response = await Client.GetBranchDashboardAsync("1", Id);
            
            // Map API response to local data structure
            _dashboard = new BranchDashboardData
            {
                BranchName = response.BranchName ?? "Unknown Branch",
                Code = response.Code,
                BranchType = response.BranchType ?? "Branch",
                Status = response.Status ?? "Active",
                OpeningDate = response.OpeningDate?.DateTime,
                
                LoanPortfolio = new LoanPortfolioData
                {
                    TotalActiveLoans = response.LoanPortfolio?.TotalActiveLoans ?? 0,
                    TotalOutstandingPrincipal = response.LoanPortfolio?.TotalOutstandingPrincipal ?? 0,
                    TotalOutstandingInterest = response.LoanPortfolio?.TotalOutstandingInterest ?? 0,
                    LoansDisbursedYTD = response.LoanPortfolio?.LoansDisbursedYTD ?? 0,
                    LoansDisbursedAmountYTD = response.LoanPortfolio?.LoansDisbursedAmountYTD ?? 0,
                    LoansFullyPaid = response.LoanPortfolio?.LoansFullyPaid ?? 0,
                    LoansOverdue = response.LoanPortfolio?.LoansOverdue ?? 0,
                    OverdueAmount = response.LoanPortfolio?.OverdueAmount ?? 0,
                    PortfolioAtRisk30 = response.LoanPortfolio?.PortfolioAtRisk30 ?? 0,
                    PortfolioAtRisk90 = response.LoanPortfolio?.PortfolioAtRisk90 ?? 0,
                    AverageLoanSize = response.LoanPortfolio?.AverageLoanSize ?? 0,
                    LoanWriteOffAmountYTD = response.LoanPortfolio?.LoanWriteOffAmountYTD ?? 0,
                    LoanApplicationsPending = response.LoanPortfolio?.LoanApplicationsPending ?? 0,
                    ApprovalRate = response.LoanPortfolio?.ApprovalRate ?? 0,
                    CollectionEfficiency = response.LoanPortfolio?.CollectionEfficiency ?? 0
                },
                
                SavingsPortfolio = new SavingsPortfolioData
                {
                    TotalSavingsAccounts = response.SavingsPortfolio?.TotalSavingsAccounts ?? 0,
                    ActiveSavingsAccounts = response.SavingsPortfolio?.ActiveSavingsAccounts ?? 0,
                    TotalSavingsBalance = response.SavingsPortfolio?.TotalSavingsBalance ?? 0,
                    SavingsDepositsYTD = response.SavingsPortfolio?.SavingsDepositsYTD ?? 0,
                    SavingsWithdrawalsYTD = response.SavingsPortfolio?.SavingsWithdrawalsYTD ?? 0,
                    AverageSavingsBalance = response.SavingsPortfolio?.AverageSavingsBalance ?? 0,
                    DormantAccounts = response.SavingsPortfolio?.DormantAccounts ?? 0,
                    TotalShareCapital = response.SavingsPortfolio?.TotalShareCapital ?? 0,
                    ShareAccountsActive = response.SavingsPortfolio?.ShareAccountsActive ?? 0
                },
                
                Members = new MemberData
                {
                    TotalMembers = response.Members?.TotalMembers ?? 0,
                    ActiveMembers = response.Members?.ActiveMembers ?? 0,
                    NewMembersYTD = response.Members?.NewMembersYTD ?? 0,
                    NewMembersLastYear = response.Members?.NewMembersLastYear ?? 0,
                    MemberGrowthPercent = (int)(response.Members?.MemberGrowthPercent ?? 0),
                    InactiveMembers = response.Members?.InactiveMembers ?? 0,
                    MembersWithLoans = response.Members?.MembersWithLoans ?? 0,
                    MembersWithSavings = response.Members?.MembersWithSavings ?? 0,
                    AverageSavingsPerMember = response.Members?.AverageSavingsPerMember ?? 0,
                    AverageLoanSizePerMember = response.Members?.AverageLoanSizePerMember ?? 0
                },
                
                Staff = new StaffData
                {
                    TotalStaff = response.Staff?.TotalStaff ?? 0,
                    ActiveStaff = response.Staff?.ActiveStaff ?? 0,
                    LoanOfficers = response.Staff?.LoanOfficers ?? 0,
                    Tellers = response.Staff?.Tellers ?? 0,
                    BranchManager = response.Staff?.BranchManager,
                    AverageLoansPerOfficer = response.Staff?.AverageLoansPerOfficer ?? 0,
                    AverageMembersPerOfficer = response.Staff?.AverageMembersPerOfficer ?? 0
                },
                
                Targets = new TargetData
                {
                    LoanDisbursementTarget = response.Targets?.LoanDisbursementTarget ?? 0,
                    LoanDisbursementActual = response.Targets?.LoanDisbursementActual ?? 0,
                    SavingsTarget = response.Targets?.SavingsTarget ?? 0,
                    SavingsActual = response.Targets?.SavingsActual ?? 0,
                    MemberTargetCount = response.Targets?.MemberTargetCount ?? 0,
                    MemberActualCount = response.Targets?.MemberActualCount ?? 0
                },
                
                RecentLoans = response.RecentLoans?.Select(loan => new RecentLoanData
                {
                    LoanNumber = loan.LoanNumber ?? "",
                    MemberName = loan.MemberName ?? "",
                    ProductName = loan.ProductName ?? "",
                    PrincipalAmount = loan.PrincipalAmount,
                    DisbursementDate = loan.DisbursementDate,
                    Status = loan.Status ?? "",
                    LoanOfficerName = loan.LoanOfficerName
                }).ToList() ?? new List<RecentLoanData>()
            };
            
            _branchName = _dashboard.BranchName;
            
            // If no data returned, use mock data
            if (_dashboard.LoanPortfolio.TotalActiveLoans == 0 && 
                _dashboard.SavingsPortfolio.TotalSavingsAccounts == 0 &&
                _dashboard.Members.TotalMembers == 0)
            {
                _dashboard = GenerateMockDashboardData();
                _branchName = _dashboard.BranchName;
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Failed to load dashboard: {ex.Message}", Severity.Error);
            _dashboard = GenerateMockDashboardData();
            _branchName = _dashboard.BranchName;
        }
        finally
        {
            _loading = false;
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
        await DialogService.ShowAsync<BranchDashboardHelpDialog>("Branch Dashboard Help", options);
    }

    private BranchDashboardData GenerateMockDashboardData()
    {
        var random = new Random();

        return new BranchDashboardData
        {
            BranchName = "Kigali Main Branch",
            Code = "BR-KGL-001",
            BranchType = "Branch",
            Status = "Active",
            OpeningDate = DateTime.Now.AddYears(-random.Next(3, 10)),

            // Loan Portfolio
            LoanPortfolio = new()
            {
                TotalActiveLoans = random.Next(200, 500),
                TotalOutstandingPrincipal = random.Next(5000000, 15000000),
                TotalOutstandingInterest = random.Next(500000, 1500000),
                LoansDisbursedYTD = random.Next(150, 400),
                LoansDisbursedAmountYTD = random.Next(3000000, 10000000),
                LoansFullyPaid = random.Next(100, 300),
                LoansOverdue = random.Next(10, 50),
                OverdueAmount = random.Next(100000, 500000),
                PortfolioAtRisk30 = (decimal)(random.NextDouble() * 8 + 2),
                PortfolioAtRisk90 = (decimal)(random.NextDouble() * 4 + 1),
                AverageLoanSize = (decimal)(random.NextDouble() * 50000 + 10000),
                CollectionEfficiency = (decimal)(random.NextDouble() * 10 + 90),
                LoanWriteOffAmountYTD = random.Next(0, 100000),
                LoanApplicationsPending = random.Next(5, 25),
                ApprovalRate = (decimal)(random.NextDouble() * 15 + 80)
            },

            // Savings Portfolio
            SavingsPortfolio = new()
            {
                TotalSavingsAccounts = random.Next(500, 1000),
                ActiveSavingsAccounts = random.Next(400, 900),
                TotalSavingsBalance = random.Next(8000000, 20000000),
                SavingsDepositsYTD = random.Next(5000000, 15000000),
                SavingsWithdrawalsYTD = random.Next(3000000, 10000000),
                AverageSavingsBalance = (decimal)(random.NextDouble() * 30000 + 10000),
                DormantAccounts = random.Next(10, 50),
                TotalShareCapital = random.Next(1000000, 5000000),
                ShareAccountsActive = random.Next(300, 700)
            },

            // Members
            Members = new()
            {
                TotalMembers = random.Next(600, 1200),
                ActiveMembers = random.Next(550, 1100),
                NewMembersYTD = random.Next(80, 200),
                NewMembersLastYear = random.Next(70, 180),
                MemberGrowthPercent = random.Next(-5, 25),
                InactiveMembers = random.Next(20, 80),
                MembersWithLoans = random.Next(250, 500),
                MembersWithSavings = random.Next(400, 900),
                AverageSavingsPerMember = (decimal)(random.NextDouble() * 20000 + 5000),
                AverageLoanSizePerMember = (decimal)(random.NextDouble() * 40000 + 10000)
            },

            // Staff
            Staff = new()
            {
                TotalStaff = random.Next(12, 25),
                ActiveStaff = random.Next(10, 23),
                LoanOfficers = random.Next(4, 8),
                Tellers = random.Next(3, 6),
                BranchManager = "John Doe",
                AverageLoansPerOfficer = (decimal)(random.NextDouble() * 30 + 20),
                AverageMembersPerOfficer = (decimal)(random.NextDouble() * 80 + 40)
            },

            // Targets
            Targets = new()
            {
                LoanDisbursementTarget = random.Next(8000000, 12000000),
                LoanDisbursementActual = random.Next(6000000, 11000000),
                SavingsTarget = random.Next(15000000, 25000000),
                SavingsActual = random.Next(12000000, 23000000),
                MemberTargetCount = random.Next(150, 250),
                MemberActualCount = random.Next(100, 220)
            },

            RecentLoans = Enumerable.Range(1, 10).Select(i => new RecentLoanData
            {
                LoanNumber = $"LN-{DateTime.Now.Year}-{random.Next(1000, 9999)}",
                MemberName = $"Member {random.Next(100, 999)}",
                ProductName = new[] { "Business Loan", "Agricultural Loan", "Personal Loan", "Education Loan" }[random.Next(4)],
                PrincipalAmount = (decimal)(random.NextDouble() * 100000 + 10000),
                DisbursementDate = DateTime.Now.AddDays(-random.Next(1, 60)),
                Status = new[] { "Disbursed", "Active", "Current", "Performing" }[random.Next(4)],
                LoanOfficerName = $"Officer {random.Next(1, 10)}"
            }).OrderByDescending(x => x.DisbursementDate).ToList()
        };
    }

    private MudBlazor.Color GetTrendColor(int value) => value >= 0 ? MudBlazor.Color.Success : MudBlazor.Color.Error;

    private MudBlazor.Color GetBranchTypeColor(string type) => type switch
    {
        "HeadOffice" or "Headquarters" => MudBlazor.Color.Primary,
        "Regional" => MudBlazor.Color.Info,
        "Branch" => MudBlazor.Color.Success,
        "SubBranch" => MudBlazor.Color.Default,
        _ => MudBlazor.Color.Default
    };

    private MudBlazor.Color GetPerformanceColor(decimal value) => value switch
    {
        >= 95 => MudBlazor.Color.Success,
        >= 85 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetPARColor(decimal par) => par switch
    {
        < 3 => MudBlazor.Color.Success,
        < 5 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private string GetPARLabel(decimal par) => par switch
    {
        < 3 => "Excellent",
        < 5 => "Good",
        < 10 => "Fair",
        _ => "Poor"
    };

    private MudBlazor.Color GetTargetAchievementColor(decimal achievement) => achievement switch
    {
        >= 100 => MudBlazor.Color.Success,
        >= 80 => MudBlazor.Color.Info,
        >= 60 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetOverdueLoanColor(int count) => count switch
    {
        < 10 => MudBlazor.Color.Success,
        < 30 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetDormantAccountColor(int count) => count switch
    {
        < 20 => MudBlazor.Color.Success,
        < 50 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private MudBlazor.Color GetLoanStatusColor(string status) => status switch
    {
        "Disbursed" or "Active" or "Current" or "Performing" => MudBlazor.Color.Success,
        "Pending" => MudBlazor.Color.Info,
        "Overdue" => MudBlazor.Color.Warning,
        "Default" or "Written Off" => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Default
    };

    // Local DTOs
    private class BranchDashboardData
    {
        public string BranchName { get; set; } = "";
        public string? Code { get; set; }
        public string BranchType { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime? OpeningDate { get; set; }

        public LoanPortfolioData LoanPortfolio { get; set; } = new();
        public SavingsPortfolioData SavingsPortfolio { get; set; } = new();
        public MemberData Members { get; set; } = new();
        public StaffData Staff { get; set; } = new();
        public TargetData Targets { get; set; } = new();

        public List<RecentLoanData> RecentLoans { get; set; } = new();
    }

    private class LoanPortfolioData
    {
        public int TotalActiveLoans { get; set; }
        public decimal TotalOutstandingPrincipal { get; set; }
        public decimal TotalOutstandingInterest { get; set; }
        public int LoansDisbursedYTD { get; set; }
        public decimal LoansDisbursedAmountYTD { get; set; }
        public int LoansFullyPaid { get; set; }
        public int LoansOverdue { get; set; }
        public decimal OverdueAmount { get; set; }
        public decimal PortfolioAtRisk30 { get; set; }
        public decimal PortfolioAtRisk90 { get; set; }
        public decimal AverageLoanSize { get; set; }
        public decimal LoanWriteOffAmountYTD { get; set; }
        public int LoanApplicationsPending { get; set; }
        public decimal ApprovalRate { get; set; }
        public decimal CollectionEfficiency { get; set; }
    }

    private class SavingsPortfolioData
    {
        public int TotalSavingsAccounts { get; set; }
        public int ActiveSavingsAccounts { get; set; }
        public decimal TotalSavingsBalance { get; set; }
        public decimal SavingsDepositsYTD { get; set; }
        public decimal SavingsWithdrawalsYTD { get; set; }
        public decimal AverageSavingsBalance { get; set; }
        public int DormantAccounts { get; set; }
        public decimal TotalShareCapital { get; set; }
        public int ShareAccountsActive { get; set; }
    }

    private class MemberData
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int NewMembersYTD { get; set; }
        public int NewMembersLastYear { get; set; }
        public int MemberGrowthPercent { get; set; }
        public int InactiveMembers { get; set; }
        public int MembersWithLoans { get; set; }
        public int MembersWithSavings { get; set; }
        public decimal AverageSavingsPerMember { get; set; }
        public decimal AverageLoanSizePerMember { get; set; }
    }

    private class StaffData
    {
        public int TotalStaff { get; set; }
        public int ActiveStaff { get; set; }
        public int LoanOfficers { get; set; }
        public int Tellers { get; set; }
        public string? BranchManager { get; set; }
        public decimal AverageLoansPerOfficer { get; set; }
        public decimal AverageMembersPerOfficer { get; set; }
    }

    private class TargetData
    {
        public decimal LoanDisbursementTarget { get; set; }
        public decimal LoanDisbursementActual { get; set; }
        public decimal LoanDisbursementAchievement => LoanDisbursementTarget > 0 
            ? (LoanDisbursementActual / LoanDisbursementTarget * 100) : 0;
        
        public decimal SavingsTarget { get; set; }
        public decimal SavingsActual { get; set; }
        public decimal SavingsAchievement => SavingsTarget > 0 
            ? (SavingsActual / SavingsTarget * 100) : 0;
        
        public int MemberTargetCount { get; set; }
        public int MemberActualCount { get; set; }
        public decimal MemberAchievement => MemberTargetCount > 0 
            ? ((decimal)MemberActualCount / MemberTargetCount * 100) : 0;
    }

    private class RecentLoanData
    {
        public string LoanNumber { get; set; } = "";
        public string MemberName { get; set; } = "";
        public string ProductName { get; set; } = "";
        public decimal PrincipalAmount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string Status { get; set; } = "";
        public string? LoanOfficerName { get; set; }
    }
}
