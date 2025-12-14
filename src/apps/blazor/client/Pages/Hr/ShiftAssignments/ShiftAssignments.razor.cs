namespace FSH.Starter.Blazor.Client.Pages.Hr.ShiftAssignments;

public partial class ShiftAssignments
{
    protected EntityServerTableContext<ShiftAssignmentResponse, DefaultIdType, ShiftAssignmentViewModel> Context { get; set; } = null!;

    private EntityTable<ShiftAssignmentResponse, DefaultIdType, ShiftAssignmentViewModel>? _table;

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

        Context = new EntityServerTableContext<ShiftAssignmentResponse, DefaultIdType, ShiftAssignmentViewModel>(
            entityName: "Shift Assignment",
            entityNamePlural: "Shift Assignments",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<ShiftAssignmentResponse>(r => r.EmployeeName ?? "-", "Employee", "EmployeeName"),
                new EntityField<ShiftAssignmentResponse>(r => r.ShiftName ?? "-", "Shift", "ShiftName"),
                new EntityField<ShiftAssignmentResponse>(r => $"{r.ShiftStartTime} - {r.ShiftEndTime}", "Time", "ShiftStartTime"),
                new EntityField<ShiftAssignmentResponse>(r => r.StartDate.ToShortDateString(), "Start Date", "StartDate"),
                new EntityField<ShiftAssignmentResponse>(r => r.EndDate?.ToShortDateString() ?? "Ongoing", "End Date", "EndDate"),
                new EntityField<ShiftAssignmentResponse>(r => r.IsRecurring ? "Yes" : "No", "Recurring", "IsRecurring"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchShiftAssignmentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchShiftAssignmentsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShiftAssignmentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateShiftAssignmentCommand
                {
                    EmployeeId = vm.SelectedEmployee?.Id ?? vm.EmployeeId,
                    ShiftId = vm.SelectedShift?.Id ?? vm.ShiftId,
                    StartDate = vm.StartDate ?? DateTime.Today,
                    EndDate = vm.EndDate,
                    IsRecurring = vm.IsRecurring,
                    Notes = vm.Notes
                };
                await Client.CreateShiftAssignmentEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateShiftAssignmentCommand
                {
                    Id = id,
                    StartDate = vm.StartDate ?? DateTime.Today,
                    EndDate = vm.EndDate,
                    IsRecurring = vm.IsRecurring,
                    Notes = vm.Notes
                };
                await Client.UpdateShiftAssignmentEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteShiftAssignmentEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<ShiftAssignmentsHelpDialog>("Shift Assignments Help", new DialogParameters(), _dialogOptions);
    }
}

public class ShiftAssignmentViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public EmployeeResponse? SelectedEmployee { get; set; }
    public DefaultIdType ShiftId { get; set; }
    public ShiftResponse? SelectedShift { get; set; }
    public DateTime? StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public string? Notes { get; set; }
}
