namespace FSH.Starter.Blazor.Client.Pages.Accounting.Accruals;

/// <summary>
/// Accruals page logic. Provides CRUD and search over Accrual entities using the generated API client.
/// Mirrors patterns used by other Accounting pages (Budgets, ChartOfAccounts).
/// </summary>
public partial class Accruals
{
    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<AccrualResponse, DefaultIdType, AccrualViewModel> Context { get; set; } = null!;

    private EntityTable<AccrualResponse, DefaultIdType, AccrualViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Advanced search filters
    private string? _searchAccrualNumber;
    private string? SearchAccrualNumber
    {
        get => _searchAccrualNumber;
        set
        {
            _searchAccrualNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchReversedOnly;
    private bool? SearchReversedOnly
    {
        get => _searchReversedOnly;
        set
        {
            _searchReversedOnly = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        Context = new EntityServerTableContext<AccrualResponse, DefaultIdType, AccrualViewModel>(
            entityName: "Accrual",
            entityNamePlural: "Accruals",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccrualResponse>(response => response.AccrualNumber, "Number", "AccrualNumber"),
                new EntityField<AccrualResponse>(response => response.AccrualDate, "Date", "AccrualDate", typeof(DateOnly)),
                new EntityField<AccrualResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<AccrualResponse>(response => response.IsReversed, "Reversed", "IsReversed", typeof(bool)),
                new EntityField<AccrualResponse>(response => response.ReversalDate, "Reversal Date", "ReversalDate", typeof(DateOnly?)),
                new EntityField<AccrualResponse>(response => response.Description, "Description", "Description"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchAccrualsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.AccrualSearchEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<AccrualResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.AccrualCreateEndpointAsync("1", viewModel.Adapt<CreateAccrualCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                if (viewModel is { IsReversed: true, ReversalDate: not null })
                {
                    await Client.AccrualReverseEndpointAsync("1", id, viewModel.Adapt<ReverseAccrualCommand>()).ConfigureAwait(false);
                }
                else
                {
                    await Client.AccrualUpdateEndpointAsync("1", id, viewModel.Adapt<UpdateAccrualCommand>()).ConfigureAwait(false);
                }
            },
            deleteFunc: async id => await Client.AccrualDeleteEndpointAsync("1", id).ConfigureAwait(false));

        await Task.CompletedTask;
    }

    private async Task OnApproveAccrual(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Approve Accrual", "Are you sure you want to approve this accrual?", yesText: "Approve", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new ApproveAccrualCommand();
                await Client.AccrualApproveEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Accrual approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve accrual: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnRejectAccrual(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Reject Accrual", "Are you sure you want to reject this accrual?", yesText: "Reject", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new RejectAccrualCommand();
                await Client.AccrualRejectEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Accrual rejected", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reject accrual: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnReverseAccrual(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Reverse Accrual", "Are you sure you want to reverse this accrual?", yesText: "Reverse", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new ReverseAccrualCommand { ReversalDate = DateTime.Today };
                await Client.AccrualReverseEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Accrual reversed successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reverse accrual: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ShowAccrualsHelp()
    {
        await DialogService.ShowAsync<AccrualsHelpDialog>("Accruals Help", new DialogParameters(), _helpDialogOptions);
    }
}
