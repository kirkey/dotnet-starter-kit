namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ReportGenerations;

/// <summary>
/// Report Generations page logic. View and manage generated reports.
/// </summary>
public partial class ReportGenerations
{
    protected EntityServerTableContext<ReportGenerationResponse, DefaultIdType, ReportGenerationViewModel> Context { get; set; } = null!;
    private EntityTable<ReportGenerationResponse, DefaultIdType, ReportGenerationViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

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

        Context = new EntityServerTableContext<ReportGenerationResponse, DefaultIdType, ReportGenerationViewModel>(
            fields:
            [
                new EntityField<ReportGenerationResponse>(dto => dto.ReportDefinitionId, "Report Definition", "ReportDefinitionId"),
                new EntityField<ReportGenerationResponse>(dto => dto.Trigger, "Trigger", "Trigger"),
                new EntityField<ReportGenerationResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<ReportGenerationResponse>(dto => dto.OutputFormat, "Format", "OutputFormat"),
                new EntityField<ReportGenerationResponse>(dto => dto.StartedAt, "Started", "StartedAt", typeof(DateTime)),
                new EntityField<ReportGenerationResponse>(dto => dto.CompletedAt, "Completed", "CompletedAt", typeof(DateTime)),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchReportGenerationsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchReportGenerationsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ReportGenerationResponse>>();
            },
            createFunc: async model =>
            {
                var request = new QueueReportGenerationCommand
                {
                    ReportDefinitionId = model.ReportDefinitionId,
                    Trigger = model.Trigger ?? "Manual",
                    OutputFormat = model.OutputFormat,
                    Parameters = model.Parameters,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    BranchId = model.BranchId
                };
                await Client.QueueReportGenerationAsync("1", request).ConfigureAwait(false);
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            entityName: "Report Generation",
            entityNamePlural: "Report Generations",
            entityResource: FshResources.ReportGenerations,
            hasExtraActionsFunc: () => true);
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        var generation = await Client.GetReportGenerationAsync("1", id).ConfigureAwait(false);
        var parameters = new DialogParameters { { "ReportGeneration", generation } };
        await DialogService.ShowAsync<ReportGenerationDetailsDialog>("Report Generation Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        }).ConfigureAwait(false);
    }

    private async Task CancelReport(DefaultIdType id)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Cancel Report",
            "Are you sure you want to cancel this report generation?",
            "Yes, Cancel",
            "No");
        if (confirmed == true)
        {
            await Client.CancelReportGenerationAsync("1", id, new CancelReportGenerationCommand()).ConfigureAwait(false);
            Snackbar.Add("Report generation cancelled", Severity.Success);
            await _table.ReloadDataAsync();
        }
    }

    private async Task DownloadReport(DefaultIdType id)
    {
        // TODO: Implement download functionality when API supports it
        await Task.CompletedTask;
        Snackbar.Add("Download functionality coming soon", Severity.Info);
    }

    /// <summary>
    /// Show report generations help dialog.
    /// </summary>
    private async Task ShowReportGenerationsHelp()
    {
        await DialogService.ShowAsync<ReportGenerationsHelpDialog>("Report Generations Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
