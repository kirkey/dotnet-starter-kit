namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.MemberGroups;

public partial class MemberGroupDashboard
{
    [Parameter]
    public Guid Id { get; set; }

    private bool _isLoading = true;
    private MemberGroupDashboardData? _dashboardData;

    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var response = await Client.GetMemberGroupDashboardAsync("1", Id);
            _dashboardData = response.Adapt<MemberGroupDashboardData>();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading dashboard: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task ShowHelpDialog()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true
        };
        await DialogService.ShowAsync<MemberGroupDashboardHelpDialog>("Member Group Dashboard Help", options);
    }

    private static string FormatCurrency(decimal value) => value.ToString("C0");

    private static MudBlazor.Color GetStatusColor(string status) => status switch
    {
        "Active" => MudBlazor.Color.Success,
        "Pending" => MudBlazor.Color.Warning,
        "Inactive" => MudBlazor.Color.Default,
        "Dissolved" => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Default
    };

    private static MudBlazor.Color GetPARColor(decimal par) => par switch
    {
        <= 1 => MudBlazor.Color.Success,
        <= 5 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private static MudBlazor.Color GetRepaymentRateColor(decimal rate) => rate switch
    {
        >= 95 => MudBlazor.Color.Success,
        >= 80 => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private static MudBlazor.Color GetActivityColor(string activityType) => activityType switch
    {
        "Loan Repayment" => MudBlazor.Color.Success,
        "Deposit" => MudBlazor.Color.Primary,
        "Withdrawal" => MudBlazor.Color.Warning,
        "Interest" => MudBlazor.Color.Info,
        _ => MudBlazor.Color.Default
    };

    private static Severity GetAlertSeverity(string severity) => severity switch
    {
        "Error" => Severity.Error,
        "Warning" => Severity.Warning,
        "Success" => Severity.Success,
        _ => Severity.Info
    };

    private MemberGroupDashboardData GetMockData()
    {
        return new MemberGroupDashboardData
        {
            GroupOverview = new GroupOverviewData
            {
                GroupId = Id,
                Code = "GRP-001",
                Name = "Sunrise Solidarity Group",
                Description = "A group of small business owners focused on mutual support",
                FormationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-2)),
                Status = "Active",
                GroupAgeInDays = 730,
                LeaderName = "John Doe",
                LeaderMemberId = Guid.NewGuid(),
                LoanOfficerName = "Jane Smith",
                LoanOfficerId = Guid.NewGuid()
            },
            MembershipStats = new MembershipStatsData
            {
                TotalMembers = 15,
                ActiveMembers = 12,
                InactiveMembers = 2,
                SuspendedMembers = 1,
                WithdrawnMembers = 0,
                Leaders = 1,
                Secretaries = 1,
                Treasurers = 1,
                RegularMembers = 9,
                AverageMembershipDurationDays = 540,
                NewMembersThisMonth = 2,
                MembersLeftThisMonth = 0
            },
            FinancialMetrics = new GroupFinancialMetricsData
            {
                LoanPortfolio = new LoanPortfolioData
                {
                    TotalLoans = 25,
                    ActiveLoans = 10,
                    OverdueLoans = 2,
                    ClosedLoans = 15,
                    TotalOutstandingPrincipal = 125000m,
                    TotalOutstandingInterest = 8500m,
                    PortfolioAtRisk30Days = 3.5m,
                    AverageLoanSize = 12500m
                },
                SavingsPortfolio = new SavingsPortfolioData
                {
                    TotalSavingsAccounts = 12,
                    ActiveSavingsAccounts = 10,
                    TotalSavingsBalance = 45000m,
                    AverageSavingsBalance = 4500m,
                    TotalDepositsThisMonth = 5500m,
                    TotalWithdrawalsThisMonth = 2000m
                },
                TotalDisbursedToGroup = 350000m,
                TotalRepaidByGroup = 225000m,
                GroupRepaymentRate = 92.5m
            },
            MeetingInfo = new MeetingInfoData
            {
                MeetingLocation = "Community Center, Room 5",
                MeetingFrequency = "Weekly",
                MeetingDay = "Wednesday",
                MeetingTime = new TimeOnly(10, 0),
                NextMeetingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                TotalMeetingsHeld = 95,
                AverageAttendanceRate = 85.5
            },
            ActivitySummary = new ActivitySummaryData
            {
                LoanApplicationsThisMonth = 3,
                LoanDisbursementsThisMonth = 2,
                LoanRepaymentsThisMonth = 18,
                TotalCollectedThisMonth = 15500m,
                NewMemberJoinsThisMonth = 2,
                MemberExitsThisMonth = 0
            },
            RecentMemberActivities = new List<RecentActivityData>
            {
                new() { MemberId = Guid.NewGuid(), MemberName = "Alice Brown", MemberCode = "MBR-001", ActivityType = "Loan Repayment", ActivityDescription = "Monthly payment", Amount = 1250m, ActivityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
                new() { MemberId = Guid.NewGuid(), MemberName = "Bob Wilson", MemberCode = "MBR-002", ActivityType = "Deposit", ActivityDescription = "Savings deposit", Amount = 500m, ActivityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-2)) },
                new() { MemberId = Guid.NewGuid(), MemberName = "Carol Davis", MemberCode = "MBR-003", ActivityType = "Loan Repayment", ActivityDescription = "Monthly payment", Amount = 850m, ActivityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) },
                new() { MemberId = Guid.NewGuid(), MemberName = "David Lee", MemberCode = "MBR-004", ActivityType = "Withdrawal", ActivityDescription = "Cash withdrawal", Amount = 200m, ActivityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-4)) },
                new() { MemberId = Guid.NewGuid(), MemberName = "Eva Martinez", MemberCode = "MBR-005", ActivityType = "Interest", ActivityDescription = "Interest posting", Amount = 45m, ActivityDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)) }
            },
            Alerts = new AlertsData
            {
                MembersWithOverdueLoans = 2,
                MembersWithDormantSavings = 1,
                InactiveMemberships = 2,
                PendingLoanApplications = 1,
                HasUpcomingMeeting = true,
                AlertsList = new List<AlertItemData>
                {
                    new() { Severity = "Error", Title = "Overdue Loans", Description = "2 member(s) have overdue loan payments", ActionUrl = null },
                    new() { Severity = "Warning", Title = "Inactive Members", Description = "2 member(s) are inactive or suspended", ActionUrl = null },
                    new() { Severity = "Info", Title = "Upcoming Meeting", Description = "Next meeting in 3 days", ActionUrl = null }
                }
            }
        };
    }

    // View Model Classes
    private class MemberGroupDashboardData
    {
        public GroupOverviewData GroupOverview { get; set; } = new();
        public MembershipStatsData MembershipStats { get; set; } = new();
        public GroupFinancialMetricsData FinancialMetrics { get; set; } = new();
        public MeetingInfoData MeetingInfo { get; set; } = new();
        public ActivitySummaryData ActivitySummary { get; set; } = new();
        public List<RecentActivityData> RecentMemberActivities { get; set; } = new();
        public AlertsData Alerts { get; set; } = new();
    }

    private class GroupOverviewData
    {
        public Guid GroupId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public DateOnly FormationDate { get; set; }
        public string Status { get; set; } = "";
        public int GroupAgeInDays { get; set; }
        public string? LeaderName { get; set; }
        public Guid? LeaderMemberId { get; set; }
        public string? LoanOfficerName { get; set; }
        public Guid? LoanOfficerId { get; set; }
    }

    private class MembershipStatsData
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get; set; }
        public int SuspendedMembers { get; set; }
        public int WithdrawnMembers { get; set; }
        public int Leaders { get; set; }
        public int Secretaries { get; set; }
        public int Treasurers { get; set; }
        public int RegularMembers { get; set; }
        public double AverageMembershipDurationDays { get; set; }
        public int NewMembersThisMonth { get; set; }
        public int MembersLeftThisMonth { get; set; }
    }

    private class GroupFinancialMetricsData
    {
        public LoanPortfolioData LoanPortfolio { get; set; } = new();
        public SavingsPortfolioData SavingsPortfolio { get; set; } = new();
        public decimal TotalDisbursedToGroup { get; set; }
        public decimal TotalRepaidByGroup { get; set; }
        public decimal GroupRepaymentRate { get; set; }
    }

    private class LoanPortfolioData
    {
        public int TotalLoans { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public int ClosedLoans { get; set; }
        public decimal TotalOutstandingPrincipal { get; set; }
        public decimal TotalOutstandingInterest { get; set; }
        public decimal PortfolioAtRisk30Days { get; set; }
        public decimal AverageLoanSize { get; set; }
    }

    private class SavingsPortfolioData
    {
        public int TotalSavingsAccounts { get; set; }
        public int ActiveSavingsAccounts { get; set; }
        public decimal TotalSavingsBalance { get; set; }
        public decimal AverageSavingsBalance { get; set; }
        public decimal TotalDepositsThisMonth { get; set; }
        public decimal TotalWithdrawalsThisMonth { get; set; }
    }

    private class MeetingInfoData
    {
        public string? MeetingLocation { get; set; }
        public string? MeetingFrequency { get; set; }
        public string? MeetingDay { get; set; }
        public TimeOnly? MeetingTime { get; set; }
        public DateOnly? NextMeetingDate { get; set; }
        public int TotalMeetingsHeld { get; set; }
        public double AverageAttendanceRate { get; set; }
    }

    private class ActivitySummaryData
    {
        public int LoanApplicationsThisMonth { get; set; }
        public int LoanDisbursementsThisMonth { get; set; }
        public int LoanRepaymentsThisMonth { get; set; }
        public decimal TotalCollectedThisMonth { get; set; }
        public int NewMemberJoinsThisMonth { get; set; }
        public int MemberExitsThisMonth { get; set; }
    }

    private class RecentActivityData
    {
        public Guid MemberId { get; set; }
        public string MemberName { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public string ActivityType { get; set; } = "";
        public string ActivityDescription { get; set; } = "";
        public decimal? Amount { get; set; }
        public DateOnly ActivityDate { get; set; }
    }

    private class AlertsData
    {
        public int MembersWithOverdueLoans { get; set; }
        public int MembersWithDormantSavings { get; set; }
        public int InactiveMemberships { get; set; }
        public int PendingLoanApplications { get; set; }
        public bool HasUpcomingMeeting { get; set; }
        public List<AlertItemData> AlertsList { get; set; } = new();
    }

    private class AlertItemData
    {
        public string Severity { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ActionUrl { get; set; }
    }
}
