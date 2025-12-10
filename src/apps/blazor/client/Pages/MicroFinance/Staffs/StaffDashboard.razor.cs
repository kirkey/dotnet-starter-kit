namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.Staffs;

public partial class StaffDashboard
{
    [Parameter]
    public Guid Id { get; set; }

    private bool _isLoading = true;
    private StaffDashboardData? _dashboardData;

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
            var response = await Client.GetStaffDashboardAsync("1", Id);
            _dashboardData = response;
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
        await DialogService.ShowAsync<StaffDashboardHelpDialog>("Staff Dashboard Help", options);
    }

    private static string FormatCurrency(decimal value) => value.ToString("C0");

    private static MudBlazor.Color GetStatusColor(string status) => status switch
    {
        "Active" => MudBlazor.Color.Success,
        "On Leave" => MudBlazor.Color.Warning,
        "Suspended" => MudBlazor.Color.Error,
        "Terminated" => MudBlazor.Color.Dark,
        "Resigned" => MudBlazor.Color.Default,
        "Retired" => MudBlazor.Color.Secondary,
        _ => MudBlazor.Color.Default
    };

    private static MudBlazor.Color GetMemberStatusColor(string status) => status switch
    {
        "Active" => MudBlazor.Color.Success,
        "Inactive" => MudBlazor.Color.Default,
        _ => MudBlazor.Color.Default
    };

    private static MudBlazor.Color GetGroupStatusColor(string status) => status switch
    {
        "Active" => MudBlazor.Color.Success,
        "Pending" => MudBlazor.Color.Warning,
        "Inactive" => MudBlazor.Color.Default,
        "Dissolved" => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Default
    };

    private static MudBlazor.Color GetPARColor(decimal parValue) => parValue switch
    {
        <= 1.0m => MudBlazor.Color.Success,
        <= 3.0m => MudBlazor.Color.Warning,
        <= 5.0m => MudBlazor.Color.Error,
        _ => MudBlazor.Color.Dark
    };

    private static MudBlazor.Color GetCollectionRateColor(decimal rate) => rate switch
    {
        >= 95.0m => MudBlazor.Color.Success,
        >= 85.0m => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private static MudBlazor.Color GetProgressColor(decimal progress) => progress switch
    {
        >= 100.0m => MudBlazor.Color.Success,
        >= 75.0m => MudBlazor.Color.Info,
        >= 50.0m => MudBlazor.Color.Warning,
        _ => MudBlazor.Color.Error
    };

    private static Severity GetAlertSeverity(string severity) => severity.ToLower() switch
    {
        "critical" => Severity.Error,
        "high" => Severity.Error,
        "warning" => Severity.Warning,
        "info" => Severity.Info,
        _ => Severity.Normal
    };

    private StaffDashboardData GetMockData()
    {
        return new StaffDashboardData
        {
            StaffInfo = new StaffInfoData
            {
                StaffId = Id,
                EmployeeNumber = "EMP-2024-001",
                FullName = "John Doe",
                Email = "john.doe@example.com",
                Phone = "+1-555-123-4567",
                Role = "Loan Officer",
                JobTitle = "Senior Loan Officer",
                Department = "Credit Department",
                Status = "Active",
                JoiningDate = new DateOnly(2020, 3, 15),
                TenureDays = 1827,
                BranchId = Guid.NewGuid(),
                BranchName = "Main Branch",
                ReportingTo = "Jane Smith"
            },
            Portfolio = new PortfolioData
            {
                TotalAssignedMembers = 150,
                ActiveMembers = 142,
                TotalAssignedGroups = 12,
                ActiveLoans = 85,
                OverdueLoans = 8,
                TotalOutstandingPrincipal = 2450000,
                TotalOutstandingInterest = 185000,
                PortfolioAtRisk30Days = 2.3m,
                SavingsAccountsManaged = 95,
                TotalSavingsBalance = 850000
            },
            Performance = new PerformanceData
            {
                LoansDisbursedThisMonth = 12,
                AmountDisbursedThisMonth = 450000,
                LoansCollectedThisMonth = 78,
                AmountCollectedThisMonth = 320000,
                CollectionRate = 94.5m,
                NewMembersRegisteredThisMonth = 8,
                LoanApplicationsProcessedThisMonth = 15,
                AverageProcessingTimeDays = 2.5m
            },
            Targets = new TargetsData
            {
                DisbursementTarget = 500000,
                DisbursementActual = 450000,
                DisbursementProgress = 90.0m,
                CollectionTarget = 350000,
                CollectionActual = 320000,
                CollectionProgress = 91.4m,
                NewMemberTarget = 10,
                NewMemberActual = 8,
                NewMemberProgress = 80.0m,
                OverdueRecoveryTarget = 5,
                OverdueRecoveryActual = 3,
                OverdueRecoveryProgress = 60.0m
            },
            RecentActivity = new RecentActivityData
            {
                LoansApprovedToday = 2,
                LoansDisbursedToday = 1,
                RepaymentsReceivedToday = 8,
                TotalCollectedToday = 45000,
                MemberVisitsToday = 5,
                GroupMeetingsThisWeek = 3
            },
            Alerts = new AlertsData
            {
                MembersWithOverdueLoans = 8,
                LoansNeedingFollowUp = 12,
                PendingApprovals = 4,
                UpcomingMeetings = 3,
                AlertsList = new List<AlertItemData>
                {
                    new() { Severity = "High", Title = "Overdue Payment", Description = "Member MBR-2024-0042 has payment overdue by 15 days" },
                    new() { Severity = "Warning", Title = "Target Behind", Description = "Collection target is behind by 9% with 5 days remaining" },
                    new() { Severity = "Info", Title = "Meeting Reminder", Description = "Group meeting scheduled for tomorrow at 10:00 AM" }
                }
            },
            AssignedMembers = new List<AssignedMemberData>
            {
                new() { MemberId = Guid.NewGuid(), MemberNumber = "MBR-2024-0001", MemberName = "Alice Johnson", Status = "Active", ActiveLoans = 2, TotalOutstanding = 45000, HasOverdue = false },
                new() { MemberId = Guid.NewGuid(), MemberNumber = "MBR-2024-0015", MemberName = "Bob Williams", Status = "Active", ActiveLoans = 1, TotalOutstanding = 25000, HasOverdue = true },
                new() { MemberId = Guid.NewGuid(), MemberNumber = "MBR-2024-0023", MemberName = "Carol Davis", Status = "Active", ActiveLoans = 1, TotalOutstanding = 30000, HasOverdue = false },
                new() { MemberId = Guid.NewGuid(), MemberNumber = "MBR-2024-0042", MemberName = "David Brown", Status = "Active", ActiveLoans = 2, TotalOutstanding = 55000, HasOverdue = true },
                new() { MemberId = Guid.NewGuid(), MemberNumber = "MBR-2024-0056", MemberName = "Emma Wilson", Status = "Active", ActiveLoans = 1, TotalOutstanding = 20000, HasOverdue = false }
            },
            AssignedGroups = new List<AssignedGroupData>
            {
                new() { GroupId = Guid.NewGuid(), GroupCode = "GRP-001", GroupName = "Sunrise Women Group", Status = "Active", MemberCount = 15, ActiveLoans = 12, TotalOutstanding = 180000 },
                new() { GroupId = Guid.NewGuid(), GroupCode = "GRP-005", GroupName = "Village Entrepreneurs", Status = "Active", MemberCount = 12, ActiveLoans = 10, TotalOutstanding = 150000 },
                new() { GroupId = Guid.NewGuid(), GroupCode = "GRP-008", GroupName = "Farmers United", Status = "Active", MemberCount = 18, ActiveLoans = 14, TotalOutstanding = 220000 }
            }
        };
    }

    // ViewModels for UI
    public class StaffDashboardData
    {
        public StaffInfoData StaffInfo { get; set; } = new();
        public PortfolioData Portfolio { get; set; } = new();
        public PerformanceData Performance { get; set; } = new();
        public TargetsData Targets { get; set; } = new();
        public RecentActivityData RecentActivity { get; set; } = new();
        public AlertsData Alerts { get; set; } = new();
        public List<AssignedMemberData> AssignedMembers { get; set; } = new();
        public List<AssignedGroupData> AssignedGroups { get; set; } = new();
    }

    public class StaffInfoData
    {
        public Guid StaffId { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Role { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly JoiningDate { get; set; }
        public int TenureDays { get; set; }
        public Guid? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? ReportingTo { get; set; }
    }

    public class PortfolioData
    {
        public int TotalAssignedMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int TotalAssignedGroups { get; set; }
        public int ActiveLoans { get; set; }
        public int OverdueLoans { get; set; }
        public decimal TotalOutstandingPrincipal { get; set; }
        public decimal TotalOutstandingInterest { get; set; }
        public decimal PortfolioAtRisk30Days { get; set; }
        public int SavingsAccountsManaged { get; set; }
        public decimal TotalSavingsBalance { get; set; }
    }

    public class PerformanceData
    {
        public int LoansDisbursedThisMonth { get; set; }
        public decimal AmountDisbursedThisMonth { get; set; }
        public int LoansCollectedThisMonth { get; set; }
        public decimal AmountCollectedThisMonth { get; set; }
        public decimal CollectionRate { get; set; }
        public int NewMembersRegisteredThisMonth { get; set; }
        public int LoanApplicationsProcessedThisMonth { get; set; }
        public decimal AverageProcessingTimeDays { get; set; }
    }

    public class TargetsData
    {
        public decimal DisbursementTarget { get; set; }
        public decimal DisbursementActual { get; set; }
        public decimal DisbursementProgress { get; set; }
        public decimal CollectionTarget { get; set; }
        public decimal CollectionActual { get; set; }
        public decimal CollectionProgress { get; set; }
        public int NewMemberTarget { get; set; }
        public int NewMemberActual { get; set; }
        public decimal NewMemberProgress { get; set; }
        public int OverdueRecoveryTarget { get; set; }
        public int OverdueRecoveryActual { get; set; }
        public decimal OverdueRecoveryProgress { get; set; }
    }

    public class RecentActivityData
    {
        public int LoansApprovedToday { get; set; }
        public int LoansDisbursedToday { get; set; }
        public int RepaymentsReceivedToday { get; set; }
        public decimal TotalCollectedToday { get; set; }
        public int MemberVisitsToday { get; set; }
        public int GroupMeetingsThisWeek { get; set; }
    }

    public class AlertsData
    {
        public int MembersWithOverdueLoans { get; set; }
        public int LoansNeedingFollowUp { get; set; }
        public int PendingApprovals { get; set; }
        public int UpcomingMeetings { get; set; }
        public List<AlertItemData> AlertsList { get; set; } = new();
    }

    public class AlertItemData
    {
        public string Severity { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
    }

    public class AssignedMemberData
    {
        public Guid MemberId { get; set; }
        public string MemberNumber { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int ActiveLoans { get; set; }
        public decimal TotalOutstanding { get; set; }
        public bool HasOverdue { get; set; }
    }

    public class AssignedGroupData
    {
        public Guid GroupId { get; set; }
        public string GroupCode { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public int ActiveLoans { get; set; }
        public decimal TotalOutstanding { get; set; }
    }
}
