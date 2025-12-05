using FSH.Starter.Blazor.Infrastructure.Api;

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
