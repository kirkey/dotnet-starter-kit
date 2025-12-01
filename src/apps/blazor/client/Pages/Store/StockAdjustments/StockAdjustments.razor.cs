namespace FSH.Starter.Blazor.Client.Pages.Store.StockAdjustments;

/// <summary>
/// StockAdjustments page logic. Provides CRUD and search over StockAdjustment entities using the generated API client.
/// Supports workflow operations: Approve adjustment for pending items.
/// </summary>
public partial class StockAdjustments
{
    

    protected EntityServerTableContext<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel> Context { get; set; } = null!;
    private EntityTable<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel> _table = null!;

    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchAdjustmentType;
    private string? SearchAdjustmentType
    {
        get => _searchAdjustmentType;
        set
        {
            _searchAdjustmentType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchIsApproved;
    private bool? SearchIsApproved
    {
        get => _searchIsApproved;
        set
        {
            _searchIsApproved = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchDateFrom;
    private DateTime? SearchDateFrom
    {
        get => _searchDateFrom;
        set
        {
            _searchDateFrom = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchDateTo;
    private DateTime? SearchDateTo
    {
        get => _searchDateTo;
        set
        {
            _searchDateTo = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        Context = new EntityServerTableContext<StockAdjustmentResponse, DefaultIdType, StockAdjustmentViewModel>(
            entityName: "Stock Adjustment",
            entityNamePlural: "Stock Adjustments",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<StockAdjustmentResponse>(x => x.AdjustmentType, "Type", "AdjustmentType"),
                new EntityField<StockAdjustmentResponse>(x => x.QuantityAdjusted, "Quantity", "QuantityAdjusted", typeof(int)),
                new EntityField<StockAdjustmentResponse>(x => x.Reason, "Reason", "Reason"),
                new EntityField<StockAdjustmentResponse>(x => x.AdjustmentDate, "Date", "AdjustmentDate", typeof(DateOnly)),
                new EntityField<StockAdjustmentResponse>(x => x.IsApproved, "Approved", "IsApproved", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
            searchFunc: async filter =>
            {
                var command = new SearchStockAdjustmentsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    AdjustmentType = SearchAdjustmentType,
                    DateFrom = SearchDateFrom,
                    DateTo = SearchDateTo
                };
                var result = await Client.SearchStockAdjustmentsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<StockAdjustmentResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateStockAdjustmentEndpointAsync("1", viewModel.Adapt<CreateStockAdjustmentCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateStockAdjustmentEndpointAsync("1", id, viewModel.Adapt<UpdateStockAdjustmentCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteStockAdjustmentEndpointAsync("1", id).ConfigureAwait(false));
        await Task.CompletedTask;
    }

    /// <summary>
    /// Approves a pending stock adjustment.
    /// </summary>
    private async Task ApproveAdjustment(DefaultIdType id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Approval",
            "Are you sure you want to approve this adjustment? This will update inventory levels.",
            yesText: "Approve",
            cancelText: "Cancel");

        if (result == true)
        {
            try
            {
                var command = new ApproveStockAdjustmentCommand();
                await Client.ApproveStockAdjustmentEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Stock adjustment approved successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve adjustment: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Show stock adjustments help dialog.
    /// </summary>
    private async Task ShowStockAdjustmentsHelp()
    {
        await DialogService.ShowAsync<StockAdjustmentsHelpDialog>("Stock Adjustments Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

/// <summary>
/// ViewModel for Stock Adjustment add/edit operations.
/// Inherits from UpdateStockAdjustmentCommand to ensure proper mapping with the API.
/// </summary>
public partial class StockAdjustmentViewModel : UpdateStockAdjustmentCommand;
