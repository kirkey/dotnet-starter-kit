namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveBalances;

/// <summary>
/// Leave Balances page for managing employee leave entitlements.
/// Provides CRUD operations for leave balance tracking.
/// </summary>
public partial class LeaveBalances
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<LeaveBalanceResponse, DefaultIdType, LeaveBalanceViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private EntityTable<LeaveBalanceResponse, DefaultIdType, LeaveBalanceViewModel>? _table;

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

        Context = new EntityServerTableContext<LeaveBalanceResponse, DefaultIdType, LeaveBalanceViewModel>(
            entityName: "Leave Balance",
            entityNamePlural: "Leave Balances",
            entityResource: FshResources.Leaves,
            fields:
            [
                new EntityField<LeaveBalanceResponse>(response => response.Year, "Year", "Year"),
                new EntityField<LeaveBalanceResponse>(response => response.OpeningBalance, "Opening", "OpeningBalance"),
                new EntityField<LeaveBalanceResponse>(response => response.AccruedDays, "Accrued", "AccruedDays"),
                new EntityField<LeaveBalanceResponse>(response => response.CarriedOverDays, "Carried", "CarriedOverDays"),
                new EntityField<LeaveBalanceResponse>(response => response.TakenDays, "Taken", "TakenDays"),
                new EntityField<LeaveBalanceResponse>(response => response.PendingDays, "Pending", "PendingDays"),
                new EntityField<LeaveBalanceResponse>(response => response.RemainingDays, "Remaining", "RemainingDays"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchLeaveBalancesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLeaveBalancesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LeaveBalanceResponse>>();
            },
            createFunc: async leaveBalance =>
            {
                await Client.CreateLeaveBalanceEndpointAsync("1", leaveBalance.Adapt<CreateLeaveBalanceCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, leaveBalance) =>
            {
                await Client.UpdateLeaveBalanceEndpointAsync("1", id, leaveBalance.Adapt<UpdateLeaveBalanceCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteLeaveBalanceEndpointAsync("1", id).ConfigureAwait(false);
            });

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the leave balances help dialog.
    /// </summary>
    private async Task ShowLeaveBalancesHelp()
    {
        await DialogService.ShowAsync<LeaveBalancesHelpDialog>("Leave Balances Help", new DialogParameters(), _helpDialogOptions);
    }
}
