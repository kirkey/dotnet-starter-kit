namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.TellerSessions;

/// <summary>
/// TellerSessions page logic. Provides search and open/close operations for TellerSession entities.
/// Manages teller cash sessions for daily operations with open/close/verify workflow.
/// </summary>
public partial class TellerSessions
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<TellerSessionResponse, DefaultIdType, TellerSessionViewModel> Context { get; set; } = null!;

    private EntityTable<TellerSessionResponse, DefaultIdType, TellerSessionViewModel> _table = null!;

    /// <summary>
    /// Authorization state for permission checks.
    /// </summary>
    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    /// <summary>
    /// Authorization service for permission checks.
    /// </summary>
    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canManage;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchStatus;
    private string? SearchStatus
    {
        get => _searchStatus;
        set
        {
            _searchStatus = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with teller session-specific configuration.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        // Load initial preference from localStorage
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

        Context = new EntityServerTableContext<TellerSessionResponse, DefaultIdType, TellerSessionViewModel>(
            fields:
            [
                new EntityField<TellerSessionResponse>(dto => dto.SessionNumber, "Session #", "SessionNumber"),
                new EntityField<TellerSessionResponse>(dto => dto.TellerName, "Teller", "TellerName"),
                new EntityField<TellerSessionResponse>(dto => dto.SessionDate, "Date", "SessionDate", typeof(DateTimeOffset)),
                new EntityField<TellerSessionResponse>(dto => dto.OpeningBalance, "Opening", "OpeningBalance", typeof(decimal)),
                new EntityField<TellerSessionResponse>(dto => dto.TotalCashIn, "Cash In", "TotalCashIn", typeof(decimal)),
                new EntityField<TellerSessionResponse>(dto => dto.TotalCashOut, "Cash Out", "TotalCashOut", typeof(decimal)),
                new EntityField<TellerSessionResponse>(dto => dto.TransactionCount, "Txn Count", "TransactionCount", typeof(int)),
                new EntityField<TellerSessionResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchTellerSessionsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchTellerSessionsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<TellerSessionResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.OpenTellerSessionAsync("1", viewModel.Adapt<OpenTellerSessionCommand>()).ConfigureAwait(false);
            },
            entityName: "Teller Session",
            entityNamePlural: "Teller Sessions",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManage);

        // Check permissions
        var state = await AuthState;
        _canManage = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show help dialog.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<TellerSessionsHelpDialog>("Teller Sessions Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// View session details in a dialog.
    /// </summary>
    private async Task ViewSessionDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { nameof(TellerSessionDetailsDialog.SessionId), id }
        };

        await DialogService.ShowAsync<TellerSessionDetailsDialog>("Session Details", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Close a teller session.
    /// </summary>
    private async Task CloseSession(DefaultIdType id)
    {
        // Fetch the session first
        var session = await Client.GetTellerSessionAsync("1", id).ConfigureAwait(false);
        if (session == null) return;
        
        var parameters = new DialogParameters
        {
            { nameof(TellerSessionCloseDialog.Session), session }
        };

        var dialog = await DialogService.ShowAsync<TellerSessionCloseDialog>("Close Session", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });

        var result = await dialog.Result;
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Verify a closed session.
    /// </summary>
    private async Task VerifySession(DefaultIdType id)
    {
        var result = await DialogService.ShowMessageBox(
            "Verify Session",
            "Are you sure you want to verify this session? This confirms the cash count is correct.",
            "Verify", cancelText: "Cancel");

        if (result == true)
        {
            await ApiHelper.ExecuteCallGuardedAsync(
                () => Client.VerifyTellerSessionAsync("1", id, new VerifyTellerSessionCommand { Id = id }),
                successMessage: "Session verified successfully.");
            await _table.ReloadDataAsync();
        }
    }
}
