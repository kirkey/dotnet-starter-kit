namespace FSH.Starter.Blazor.Client.Pages.Hr.Holidays;

/// <summary>
/// Holidays page for managing company holidays and special days.
/// Provides CRUD operations with Philippine Labor Code compliance.
/// </summary>
public partial class Holidays
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<HolidayResponse, DefaultIdType, HolidayViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<HolidayResponse, DefaultIdType, HolidayViewModel>? _table;

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

        Context = new EntityServerTableContext<HolidayResponse, DefaultIdType, HolidayViewModel>(
            entityName: "Holiday",
            entityNamePlural: "Holidays",
            entityResource: FshResources.Attendance,
            fields:
            [
                new EntityField<HolidayResponse>(response => response.HolidayName, "Holiday Name", "HolidayName"),
                new EntityField<HolidayResponse>(response => response.HolidayDate, "Date", "HolidayDate", typeof(DateOnly)),
                new EntityField<HolidayResponse>(response => response.Type, "Type", "Type"),
                new EntityField<HolidayResponse>(response => response.PayRateMultiplier, "Pay Rate", "PayRateMultiplier", typeof(decimal)),
                new EntityField<HolidayResponse>(response => response.IsPaid, "Paid", "IsPaid", typeof(bool)),
                new EntityField<HolidayResponse>(response => response.IsRecurringAnnually, "Recurring", "IsRecurringAnnually", typeof(bool)),
                new EntityField<HolidayResponse>(response => response.IsNationwide, "Nationwide", "IsNationwide", typeof(bool)),
                new EntityField<HolidayResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchHolidaysRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchHolidaysEndpointAsync("1", request).ConfigureAwait(false);

                return result.Adapt<PaginationResponse<HolidayResponse>>();
            },
            createFunc: async holiday =>
            {
                await Client.CreateHolidayEndpointAsync("1", holiday.Adapt<CreateHolidayCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, holiday) =>
            {
                await Client.UpdateHolidayEndpointAsync("1", id, holiday.Adapt<UpdateHolidayCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteHolidayEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the holidays help dialog.
    /// </summary>
    private async Task ShowHolidaysHelp()
    {
        await DialogService.ShowAsync<HolidaysHelpDialog>("Holidays Help", new DialogParameters(), _helpDialogOptions);
    }
}
