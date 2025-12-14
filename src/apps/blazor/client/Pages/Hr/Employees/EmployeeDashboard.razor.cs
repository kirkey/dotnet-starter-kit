namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees;

public partial class EmployeeDashboard
{
    [Parameter]
    public Guid? Id { get; set; }

    private bool _loading = true;
    private string _employeeName = "Loading...";
    private EmployeeDashboardData _dashboard = new();
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
            new("Home", "/", false, Icons.Material.Filled.Home),
            new("HR", "/hr", false, Icons.Material.Filled.Work),
            new("Employees", "/hr/employees", false, Icons.Material.Filled.People),
            new("Dashboard", null, true, Icons.Material.Filled.Dashboard)
        };

        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        _loading = true;
        StateHasChanged();

        try
        {
            var response = Id.HasValue 
                ? await Client.GetTeamDashboardEndpointAsync("1", Id.Value)
                : await Client.GetEmployeeDashboardEndpointAsync("1");

            if (response != null && response.PersonalSummary != null && !string.IsNullOrEmpty(response.PersonalSummary.FirstName))
            {
                _dashboard = response.Adapt<EmployeeDashboardData>();
                _employeeName = $"{_dashboard.PersonalSummary.FirstName} {_dashboard.PersonalSummary.LastName}";
            }
            else
            {
                _dashboard = GenerateMockData();
                _employeeName = $"{_dashboard.PersonalSummary.FirstName} {_dashboard.PersonalSummary.LastName}";
                Snackbar.Add("Using sample data for demonstration", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            _dashboard = GenerateMockData();
            _employeeName = $"{_dashboard.PersonalSummary.FirstName} {_dashboard.PersonalSummary.LastName}";
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
        await DialogService.ShowAsync<EmployeeDashboardHelpDialog>("Employee Dashboard Help", options);
    }

    private static MudBlazor.Color GetStatusColor(string? status)
    {
        return status?.ToUpperInvariant() switch
        {
            "ACTIVE" => MudBlazor.Color.Success,
            "ON LEAVE" => MudBlazor.Color.Info,
            "SUSPENDED" => MudBlazor.Color.Warning,
            "TERMINATED" => MudBlazor.Color.Error,
            "RESIGNED" => MudBlazor.Color.Default,
            _ => MudBlazor.Color.Default
        };
    }

    private static MudBlazor.Color GetRatingColor(decimal rating)
    {
        return rating switch
        {
            >= 4.5m => MudBlazor.Color.Success,
            >= 3.5m => MudBlazor.Color.Info,
            >= 2.5m => MudBlazor.Color.Warning,
            _ => MudBlazor.Color.Error
        };
    }

    private EmployeeDashboardData GenerateMockData()
    {
        var random = new Random();
        return new EmployeeDashboardData
        {
            EmployeeId = Id ?? Guid.NewGuid(),
            PersonalSummary = new PersonalSummaryData
            {
                EmployeeId = Id ?? Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@company.com",
                PhoneNumber = "+1-555-123-4567",
                Designation = "Senior Software Engineer",
                Department = "Engineering",
                ProfilePhotoUrl = "",
                JoinDate = DateTime.Now.AddYears(-3),
                EmploymentStatus = "Active"
            },
            LeaveMetrics = new LeaveMetricsData
            {
                TotalEntitlement = 20,
                AvailableLeaveDays = 12.5m,
                UsedLeaveDays = 7.5m,
                PendingLeaveDays = 2,
                ApprovedLeaveDays = 7.5m,
                RejectedLeaveDays = 0.5m
            },
            AttendanceMetrics = new AttendanceMetricsData
            {
                TotalDaysThisMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month),
                PresentDaysThisMonth = random.Next(18, 22),
                AbsentDaysThisMonth = random.Next(0, 3),
                LateDaysThisMonth = random.Next(0, 5),
                AttendanceRate = 92.5m + (decimal)(random.NextDouble() * 5)
            },
            PayrollSnapshot = new PayrollSnapshotData
            {
                LastPayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1),
                LastPayAmount = random.Next(40000, 80000),
                GrossPayYTD = random.Next(400000, 800000),
                NetPayYTD = random.Next(300000, 600000),
                TotalDeductionsYTD = random.Next(50000, 150000),
                TaxWithheldYTD = random.Next(40000, 120000)
            },
            PendingApprovals = new PendingApprovalsData
            {
                PendingLeaveRequests = random.Next(0, 3),
                PendingTimesheets = random.Next(0, 2),
                PendingDocuments = random.Next(0, 4),
                TotalPending = 0
            },
            PerformanceSnapshot = new PerformanceSnapshotData
            {
                AverageRating = 4.0m + (decimal)(random.NextDouble() * 1),
                TotalReviews = random.Next(2, 6),
                LastReviewDate = DateTime.Now.AddMonths(-random.Next(1, 6)),
                NextReviewDate = DateTime.Now.AddMonths(random.Next(1, 6))
            },
            UpcomingSchedule = new UpcomingScheduleData
            {
                NextShiftDate = DateTime.Now.AddDays(1),
                NextShiftTime = "9:00 AM - 5:00 PM",
                UpcomingHolidays = random.Next(1, 5),
                ScheduledLeaveDays = random.Next(0, 3)
            },
            QuickActions = new QuickActionsData
            {
                CanSubmitLeave = true,
                CanClockIn = DateTime.Now.Hour < 10,
                CanClockOut = DateTime.Now.Hour >= 17,
                CanUploadDocument = true,
                CanSubmitTimesheet = true,
                NextActionMessage = "Remember to submit your timesheet by end of week"
            },
            GeneratedAt = DateTime.Now
        };
    }

    // View Model Classes
    private class EmployeeDashboardData
    {
        public Guid EmployeeId { get; set; }
        public PersonalSummaryData PersonalSummary { get; set; } = new();
        public LeaveMetricsData LeaveMetrics { get; set; } = new();
        public AttendanceMetricsData AttendanceMetrics { get; set; } = new();
        public PayrollSnapshotData PayrollSnapshot { get; set; } = new();
        public PendingApprovalsData PendingApprovals { get; set; } = new();
        public PerformanceSnapshotData PerformanceSnapshot { get; set; } = new();
        public UpcomingScheduleData UpcomingSchedule { get; set; } = new();
        public QuickActionsData QuickActions { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    private class PersonalSummaryData
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string ProfilePhotoUrl { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
    }

    private class LeaveMetricsData
    {
        public decimal TotalEntitlement { get; set; }
        public decimal AvailableLeaveDays { get; set; }
        public decimal UsedLeaveDays { get; set; }
        public decimal PendingLeaveDays { get; set; }
        public decimal ApprovedLeaveDays { get; set; }
        public decimal RejectedLeaveDays { get; set; }
    }

    private class AttendanceMetricsData
    {
        public int TotalDaysThisMonth { get; set; }
        public int PresentDaysThisMonth { get; set; }
        public int AbsentDaysThisMonth { get; set; }
        public int LateDaysThisMonth { get; set; }
        public decimal AttendanceRate { get; set; }
    }

    private class PayrollSnapshotData
    {
        public DateTime LastPayDate { get; set; }
        public decimal LastPayAmount { get; set; }
        public decimal GrossPayYTD { get; set; }
        public decimal NetPayYTD { get; set; }
        public decimal TotalDeductionsYTD { get; set; }
        public decimal TaxWithheldYTD { get; set; }
    }

    private class PendingApprovalsData
    {
        public int PendingLeaveRequests { get; set; }
        public int PendingTimesheets { get; set; }
        public int PendingDocuments { get; set; }
        public int TotalPending { get; set; }
    }

    private class PerformanceSnapshotData
    {
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public DateTime LastReviewDate { get; set; }
        public DateTime NextReviewDate { get; set; }
    }

    private class UpcomingScheduleData
    {
        public DateTime NextShiftDate { get; set; }
        public string NextShiftTime { get; set; } = string.Empty;
        public int UpcomingHolidays { get; set; }
        public int ScheduledLeaveDays { get; set; }
    }

    private class QuickActionsData
    {
        public bool CanSubmitLeave { get; set; }
        public bool CanClockIn { get; set; }
        public bool CanClockOut { get; set; }
        public bool CanUploadDocument { get; set; }
        public bool CanSubmitTimesheet { get; set; }
        public string NextActionMessage { get; set; } = string.Empty;
    }
}
