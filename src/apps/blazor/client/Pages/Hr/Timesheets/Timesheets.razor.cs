namespace FSH.Starter.Blazor.Client.Pages.Hr.Timesheets;

/// <summary>
/// Timesheets page for managing employee time tracking.
/// Provides CRUD operations for timesheet management.
/// </summary>
public partial class Timesheets
{
    

    protected EntityServerTableContext<TimesheetResponse, DefaultIdType, TimesheetViewModel> Context { get; set; } = null!;

    private EntityTable<TimesheetResponse, DefaultIdType, TimesheetViewModel>? _table;
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<TimesheetResponse, DefaultIdType, TimesheetViewModel>(
            entityName: "Timesheet",
            entityNamePlural: "Timesheets",
            entityResource: FshResources.Timesheets,
            fields:
            [
                new EntityField<TimesheetResponse>(response => response.StartDate.ToShortDateString(), "Start Date", "StartDate"),
                new EntityField<TimesheetResponse>(response => response.EndDate.ToShortDateString(), "End Date", "EndDate"),
                new EntityField<TimesheetResponse>(response => response.PeriodType ?? "Weekly", "Period", "PeriodType"),
                new EntityField<TimesheetResponse>(response => response.RegularHours, "Regular Hrs", "RegularHours"),
                new EntityField<TimesheetResponse>(response => response.OvertimeHours, "OT Hrs", "OvertimeHours"),
                new EntityField<TimesheetResponse>(response => response.TotalHours, "Total Hrs", "TotalHours"),
                new EntityField<TimesheetResponse>(response => response.Status ?? "Draft", "Status", "Status"),
                new EntityField<TimesheetResponse>(response => response.IsApproved, "Approved", "IsApproved", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchTimesheetsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchTimesheetsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<TimesheetResponse>>();
            },
            createFunc: async timesheet =>
            {
                await Client.CreateTimesheetEndpointAsync("1", timesheet.Adapt<CreateTimesheetCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, timesheet) =>
            {
                await Client.UpdateTimesheetEndpointAsync("1", id, timesheet.Adapt<UpdateTimesheetCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteTimesheetEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the timesheets help dialog.
    /// </summary>
    private async Task ShowTimesheetsHelp()
    {
        await DialogService.ShowAsync<TimesheetsHelpDialog>("Timesheets Help", new DialogParameters(), _helpDialogOptions);
    }

    /// <summary>
    /// Submits a timesheet for approval.
    /// NOTE: This functionality requires API client regeneration.
    /// </summary>
    private async Task SubmitTimesheetAsync(TimesheetResponse timesheet)
    {
        Snackbar.Add("Submit action requires API client regeneration", Severity.Warning);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Approves a submitted timesheet.
    /// NOTE: This functionality requires API client regeneration.
    /// </summary>
    private async Task ApproveTimesheetAsync(TimesheetResponse timesheet)
    {
        Snackbar.Add("Approve action requires API client regeneration", Severity.Warning);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Rejects a submitted timesheet.
    /// NOTE: This functionality requires API client regeneration.
    /// </summary>
    private async Task RejectTimesheetAsync(TimesheetResponse timesheet)
    {
        Snackbar.Add("Reject action requires API client regeneration", Severity.Warning);
        await Task.CompletedTask;
    }
}
