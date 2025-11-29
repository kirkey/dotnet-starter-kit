namespace FSH.Starter.Blazor.Client.Pages.Hr.Shifts;

/// <summary>
/// Shifts page for managing work shift templates and schedules.
/// Provides CRUD operations for shift definitions.
/// </summary>
public partial class Shifts
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<ShiftResponse, DefaultIdType, ShiftViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<ShiftResponse, DefaultIdType, ShiftViewModel>? _table;

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
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<ShiftResponse, DefaultIdType, ShiftViewModel>(
            entityName: "Shift",
            entityNamePlural: "Shifts",
            entityResource: FshResources.Attendance,
            fields:
            [
                new EntityField<ShiftResponse>(response => response.ShiftName, "Shift Name", "ShiftName"),
                new EntityField<ShiftResponse>(response => $"{response.StartTime:hh\\:mm}", "Start Time", "StartTime"),
                new EntityField<ShiftResponse>(response => $"{response.EndTime:hh\\:mm}", "End Time", "EndTime"),
                new EntityField<ShiftResponse>(response => response.WorkingHours, "Working Hours", "WorkingHours", typeof(decimal)),
                new EntityField<ShiftResponse>(response => response.BreakDurationMinutes, "Break (min)", "BreakDurationMinutes", typeof(int)),
                new EntityField<ShiftResponse>(response => response.IsOvernight, "Overnight", "IsOvernight", typeof(bool)),
                new EntityField<ShiftResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchShiftsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShiftsEndpointAsync("1", request).ConfigureAwait(false);

                return result.Adapt<PaginationResponse<ShiftResponse>>();
            },
            createFunc: async shift =>
            {
                await Client.CreateShiftEndpointAsync("1", shift.Adapt<CreateShiftCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, shift) =>
            {
                await Client.UpdateShiftEndpointAsync("1", id, shift.Adapt<UpdateShiftCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteShiftEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the shifts help dialog.
    /// </summary>
    private async Task ShowShiftsHelp()
    {
        await DialogService.ShowAsync<ShiftsHelpDialog>("Shifts Help", new DialogParameters(), _helpDialogOptions);
    }
}
