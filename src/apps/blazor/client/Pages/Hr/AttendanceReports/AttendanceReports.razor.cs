namespace FSH.Starter.Blazor.Client.Pages.Hr.AttendanceReports;

public partial class AttendanceReports
{
    protected EntityServerTableContext<AttendanceReportDto, DefaultIdType, AttendanceReportViewModel> Context { get; set; } = null!;

    private EntityTable<AttendanceReportDto, DefaultIdType, AttendanceReportViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _dialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true 
    };

    // Generate report dialog state
    private bool _generateDialogVisible;
    private GenerateReportDialogState _generateCommand = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<AttendanceReportDto, DefaultIdType, AttendanceReportViewModel>(
            entityName: "Attendance Report",
            entityNamePlural: "Attendance Reports",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<AttendanceReportDto>(r => r.Title ?? "-", "Title", "Title"),
                new EntityField<AttendanceReportDto>(r => r.ReportType ?? "-", "Type", "ReportType"),
                new EntityField<AttendanceReportDto>(r => r.FromDate.ToShortDateString(), "From", "FromDate"),
                new EntityField<AttendanceReportDto>(r => r.ToDate.ToShortDateString(), "To", "ToDate"),
                new EntityField<AttendanceReportDto>(r => $"{r.AttendancePercentage:F1}%", "Attendance %", "AttendancePercentage"),
                new EntityField<AttendanceReportDto>(r => r.TotalEmployees.ToString(), "Employees", "TotalEmployees"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchAttendanceReportsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchAttendanceReportsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AttendanceReportDto>>();
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<AttendanceReportsHelpDialog>("Attendance Reports Help", new DialogParameters(), _dialogOptions);
    }

    private void ShowGenerateDialog()
    {
        _generateCommand = new GenerateReportDialogState
        {
            Title = $"Attendance Report - {DateTime.Today:yyyy-MM}",
            ReportType = "Summary",
            FromDate = DateTime.Today.AddMonths(-1),
            ToDate = DateTime.Today
        };
        _generateDialogVisible = true;
    }

    private async Task SubmitGenerateReport()
    {
        try
        {
            var command = new GenerateAttendanceReportCommand
            {
                Title = _generateCommand.Title,
                ReportType = _generateCommand.ReportType,
                FromDate = _generateCommand.FromDate,
                ToDate = _generateCommand.ToDate,
                DepartmentId = _generateCommand.DepartmentId,
                EmployeeId = _generateCommand.EmployeeId,
                Notes = _generateCommand.Notes
            };
            await Client.GenerateAttendanceReportEndpointAsync("1", command).ConfigureAwait(false);
            Snackbar.Add("Attendance report generated successfully", Severity.Success);
            _generateDialogVisible = false;
            if (_table != null)
                await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error generating report: {ex.Message}", Severity.Error);
        }
    }

    private async Task ExportReportAsync(DefaultIdType reportId, string format)
    {
        try
        {
            var request = new ExportAttendanceReportRequest { Format = format };
            await Client.ExportAttendanceReportEndpointAsync("1", reportId, request).ConfigureAwait(false);
            Snackbar.Add($"Report exported to {format.ToUpper()}", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error exporting report: {ex.Message}", Severity.Error);
        }
    }

    private async Task ViewReportDetailsAsync(AttendanceReportDto report)
    {
        var parameters = new DialogParameters
        {
            { nameof(AttendanceReportDetailsDialog.Report), report }
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<AttendanceReportDetailsDialog>("Attendance Report Details", parameters, options);
    }

    private async Task DownloadReportAsync(AttendanceReportDto report)
    {
        if (!string.IsNullOrEmpty(report.ExportPath))
        {
            Snackbar.Add($"Downloading report: {report.Title}", Severity.Info);
            // In a real implementation, this would trigger a file download
            await Task.CompletedTask;
        }
    }
}

public class AttendanceReportViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Title { get; set; }
    public string? ReportType { get; set; } = "Monthly";
    public DateTime? FromDate { get; set; } = DateTime.Today.AddMonths(-1);
    public DateTime? ToDate { get; set; } = DateTime.Today;
}

public class GenerateReportDialogState
{
    public string? Title { get; set; }
    public string? ReportType { get; set; } = "Summary";
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DefaultIdType? DepartmentId { get; set; }
    public DefaultIdType? EmployeeId { get; set; }
    public string? Notes { get; set; }
}
