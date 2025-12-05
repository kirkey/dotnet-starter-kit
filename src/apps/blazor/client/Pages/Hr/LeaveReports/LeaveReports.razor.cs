using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveReports;

public partial class LeaveReports
{
    protected EntityServerTableContext<LeaveReportDto, DefaultIdType, LeaveReportViewModel> Context { get; set; } = null!;

    private EntityTable<LeaveReportDto, DefaultIdType, LeaveReportViewModel>? _table;

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

        Context = new EntityServerTableContext<LeaveReportDto, DefaultIdType, LeaveReportViewModel>(
            entityName: "Leave Report",
            entityNamePlural: "Leave Reports",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<LeaveReportDto>(r => r.Title ?? "-", "Title", "Title"),
                new EntityField<LeaveReportDto>(r => r.ReportType ?? "-", "Type", "ReportType"),
                new EntityField<LeaveReportDto>(r => r.FromDate.ToShortDateString(), "From", "FromDate"),
                new EntityField<LeaveReportDto>(r => r.ToDate.ToShortDateString(), "To", "ToDate"),
                new EntityField<LeaveReportDto>(r => r.TotalLeaveRequests.ToString(), "Requests", "TotalLeaveRequests"),
                new EntityField<LeaveReportDto>(r => r.ApprovedLeaveCount.ToString(), "Approved", "ApprovedLeaveCount"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchLeaveReportsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchLeaveReportsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LeaveReportDto>>();
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<LeaveReportsHelpDialog>("Leave Reports Help", new DialogParameters(), _dialogOptions);
    }

    private async Task ViewReportDetailsAsync(LeaveReportDto report)
    {
        var parameters = new DialogParameters
        {
            { nameof(LeaveReportDetailsDialog.Report), report }
        };
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<LeaveReportDetailsDialog>("Leave Report Details", parameters, options);
    }

    private async Task DownloadReportAsync(LeaveReportDto report)
    {
        if (!string.IsNullOrEmpty(report.ExportPath))
        {
            Snackbar.Add($"Downloading report: {report.Title}", Severity.Info);
            await Task.CompletedTask;
        }
    }
}

public class LeaveReportViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Title { get; set; }
    public string? ReportType { get; set; } = "Monthly";
    public DateTime? FromDate { get; set; } = DateTime.Today.AddMonths(-1);
    public DateTime? ToDate { get; set; } = DateTime.Today;
}
