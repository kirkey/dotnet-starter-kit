using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.TimesheetLines;

public partial class TimesheetLines
{
    protected EntityServerTableContext<TimesheetLineResponse, DefaultIdType, TimesheetLineViewModel> Context { get; set; } = null!;

    private EntityTable<TimesheetLineResponse, DefaultIdType, TimesheetLineViewModel>? _table;

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

        Context = new EntityServerTableContext<TimesheetLineResponse, DefaultIdType, TimesheetLineViewModel>(
            entityName: "Timesheet Line",
            entityNamePlural: "Timesheet Lines",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<TimesheetLineResponse>(r => r.WorkDate.ToShortDateString(), "Date", "WorkDate"),
                new EntityField<TimesheetLineResponse>(r => r.ProjectId ?? "-", "Project", "ProjectId"),
                new EntityField<TimesheetLineResponse>(r => r.RegularHours.ToString("F1"), "Reg Hours", "RegularHours"),
                new EntityField<TimesheetLineResponse>(r => r.OvertimeHours.ToString("F1"), "OT Hours", "OvertimeHours"),
                new EntityField<TimesheetLineResponse>(r => r.TotalHours.ToString("F1"), "Total", "TotalHours"),
                new EntityField<TimesheetLineResponse>(r => r.IsBillable ? "Yes" : "No", "Billable", "IsBillable"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchTimesheetLinesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchTimesheetLinesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<TimesheetLineResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateTimesheetLineCommand
                {
                    TimesheetId = vm.TimesheetId,
                    WorkDate = vm.WorkDate ?? DateTime.Today,
                    RegularHours = vm.RegularHours ?? 8,
                    OvertimeHours = vm.OvertimeHours ?? 0,
                    ProjectId = vm.ProjectId,
                    TaskDescription = vm.TaskDescription,
                    BillingRate = vm.BillingRate
                };
                await Client.CreateTimesheetLineEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateTimesheetLineCommand
                {
                    Id = id,
                    RegularHours = vm.RegularHours,
                    OvertimeHours = vm.OvertimeHours,
                    ProjectId = vm.ProjectId,
                    TaskDescription = vm.TaskDescription,
                    IsBillable = vm.IsBillable,
                    BillingRate = vm.BillingRate
                };
                await Client.UpdateTimesheetLineEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteTimesheetLineEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<TimesheetLinesHelpDialog>("Timesheet Lines Help", new DialogParameters(), _dialogOptions);
    }
}

public class TimesheetLineViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType TimesheetId { get; set; }
    public DateTime? WorkDate { get; set; } = DateTime.Today;
    public decimal? RegularHours { get; set; } = 8;
    public decimal? OvertimeHours { get; set; } = 0;
    public string? ProjectId { get; set; }
    public string? TaskDescription { get; set; }
    public bool IsBillable { get; set; }
    public decimal? BillingRate { get; set; }
}
