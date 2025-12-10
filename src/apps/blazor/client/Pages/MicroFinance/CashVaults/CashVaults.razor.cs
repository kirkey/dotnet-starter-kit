namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CashVaults;

/// <summary>
/// CashVaults page logic. Provides CRUD and search over CashVault entities using the generated API client.
/// Manages cash vaults including main vaults, branch vaults, and ATM cash holdings.
/// </summary>
public partial class CashVaults
{
    static CashVaults()
    {
        // Configure Mapster to convert DateTimeOffset? to DateTime? for CashVaultResponse -> CashVaultViewModel mapping
        TypeAdapterConfig<CashVaultResponse, CashVaultViewModel>.NewConfig()
            .Map(dest => dest.LastReconciliationDate, src => src.LastReconciliationDate.HasValue ? src.LastReconciliationDate.Value.DateTime : (DateTime?)null);
    }

    /// <summary>
    /// Table context that drives the generic <see cref="EntityTable{TEntity, TId, TRequest}"/> used in the Razor view.
    /// </summary>
    protected EntityServerTableContext<CashVaultResponse, DefaultIdType, CashVaultViewModel> Context { get; set; } = null!;

    private EntityTable<CashVaultResponse, DefaultIdType, CashVaultViewModel> _table = null!;

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
    private bool _canManageVaults;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchCode;
    private string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchName;
    private string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

    private string? _searchVaultType;
    private string? SearchVaultType
    {
        get => _searchVaultType;
        set
        {
            _searchVaultType = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Initializes the table context with cash vault-specific configuration including fields, CRUD operations, and search functionality.
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

        Context = new EntityServerTableContext<CashVaultResponse, DefaultIdType, CashVaultViewModel>(
            fields:
            [
                new EntityField<CashVaultResponse>(dto => dto.Code, "Vault Code", "Code"),
                new EntityField<CashVaultResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<CashVaultResponse>(dto => dto.VaultType, "Type", "VaultType"),
                new EntityField<CashVaultResponse>(dto => dto.CurrentBalance, "Current Balance", "CurrentBalance", typeof(decimal)),
                new EntityField<CashVaultResponse>(dto => dto.MinimumBalance, "Min Balance", "MinimumBalance", typeof(decimal)),
                new EntityField<CashVaultResponse>(dto => dto.MaximumBalance, "Max Balance", "MaximumBalance", typeof(decimal)),
                new EntityField<CashVaultResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CashVaultResponse>(dto => dto.LastReconciliationDate, "Last Reconciled", "LastReconciliationDate", typeof(DateTime)),
            ],
            searchFunc: async filter =>
            {
                // TODO: Implement SearchCashVaultsAsync in the API once available
                // For now, return empty pagination response
                await Task.CompletedTask;
                return new PaginationResponse<CashVaultResponse>
                {
                    Items = new List<CashVaultResponse>(),
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalCount = 0
                };
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateCashVaultAsync("1", viewModel.Adapt<CreateCashVaultCommand>()).ConfigureAwait(false);
            },
            // Note: UpdateCashVaultAsync is not yet implemented in the API
            // updateFunc will be null, disabling edit functionality until API is ready
            getDefaultsFunc: async () => await Task.FromResult(new CashVaultViewModel
            {
                VaultType = "BranchVault",
                Status = "Active",
                MinimumBalance = 10000M,
                MaximumBalance = 1000000M,
                OpeningBalance = 100000M
            }),
            entityName: "Cash Vault",
            entityNamePlural: "Cash Vaults",
            entityResource: FshResources.MicroFinance,
            hasExtraActionsFunc: () => _canManageVaults);

        // Check permissions for extra actions
        var state = await AuthState;
        _canManageVaults = await AuthService.HasPermissionAsync(state.User, FshActions.Update, FshResources.MicroFinance);
    }

    /// <summary>
    /// Show cash vault help dialog.
    /// </summary>
    private async Task ShowCashVaultsHelp()
    {
        await DialogService.ShowMessageBox(
            "Cash Vault Management Help",
            (MarkupString)GetHelpContent(),
            yesText: "Close");
    }

    private static string GetHelpContent() => """
        <h4>Cash Vault Management</h4>
        <p>This page allows you to manage cash vaults across all branches.</p>
        
        <h5>Vault Types</h5>
        <ul>
            <li><strong>Main Vault</strong> - Primary vault at headquarters</li>
            <li><strong>Branch Vault</strong> - Standard branch vault</li>
            <li><strong>ATM</strong> - Cash holdings for ATM machines</li>
            <li><strong>Safe Deposit</strong> - Secure deposit vault</li>
        </ul>
        
        <h5>Vault Status</h5>
        <ul>
            <li><strong>Active</strong> - Vault is operational</li>
            <li><strong>Suspended</strong> - Vault operations temporarily suspended</li>
            <li><strong>Closed</strong> - Vault permanently closed</li>
        </ul>
        
        <h5>Workflow Actions</h5>
        <ul>
            <li><strong>Close Day</strong> - End of day closure with balance verification</li>
            <li><strong>Reconcile</strong> - Perform physical cash count reconciliation</li>
            <li><strong>Transfer</strong> - Transfer cash between vaults</li>
        </ul>
        
        <h5>Balance Thresholds</h5>
        <ul>
            <li><strong>Minimum Balance</strong> - Alert if balance falls below this</li>
            <li><strong>Maximum Balance</strong> - Excess should be transferred out</li>
        </ul>
        """;

    /// <summary>
    /// View details of a specific cash vault.
    /// </summary>
    private async Task ViewVaultDetails(DefaultIdType id)
    {
        var vault = await Client.GetCashVaultAsync("1", id).ConfigureAwait(false);
        if (vault != null)
        {
            var parameters = new DialogParameters
            {
                { "Vault", vault }
            };
            await DialogService.ShowAsync<CashVaultDetailsDialog>("Cash Vault Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    /// <summary>
    /// Close the day for a cash vault.
    /// </summary>
    private async Task CloseDayVault(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { "VaultId", id },
            { "OnConfirm", new Func<decimal, string?, Task>(async (verifiedBalance, denominationBreakdown) =>
                {
                    var command = new CloseDayCashVaultCommand
                    {
                        CashVaultId = id,
                        VerifiedBalance = verifiedBalance,
                        DenominationBreakdown = denominationBreakdown
                    };
                    
                    await ApiHelper.ExecuteCallGuardedAsync(
                        () => Client.CloseDayVaultAsync("1", id, command),
                        successMessage: "Day closed successfully for the vault.");
                    
                    await _table.ReloadDataAsync();
                })
            }
        };

        await DialogService.ShowAsync<CloseDayVaultDialog>("Close Day", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Reconcile a cash vault.
    /// </summary>
    private async Task ReconcileVault(DefaultIdType id)
    {
        var parameters = new DialogParameters
        {
            { "VaultId", id },
            { "OnConfirm", new Func<decimal, string?, Task>(async (physicalCount, denominationBreakdown) =>
                {
                    var command = new ReconcileCashVaultCommand
                    {
                        Id = id,
                        PhysicalCount = physicalCount,
                        DenominationBreakdown = denominationBreakdown
                    };
                    
                    var result = await ApiHelper.ExecuteCallGuardedAsync(
                        () => Client.ReconcileVaultAsync("1", id, command),
                        successMessage: "Vault reconciliation completed.");
                    
                    if (result != null)
                    {
                        var message = $"Reconciliation completed.\n" +
                                      $"Expected: {result.ExpectedBalance:C}\n" +
                                      $"Actual: {result.ActualBalance:C}\n" +
                                      $"Variance: {result.Variance:C}";
                        
                        if (result.Variance != 0)
                        {
                            Snackbar.Add(message, Severity.Warning);
                        }
                    }
                    
                    await _table.ReloadDataAsync();
                })
            }
        };

        await DialogService.ShowAsync<ReconcileVaultDialog>("Reconcile Vault", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    /// <summary>
    /// Transfer cash to another vault.
    /// </summary>
    private async Task TransferToVault(DefaultIdType sourceVaultId)
    {
        var parameters = new DialogParameters
        {
            { "SourceVaultId", sourceVaultId },
            { "OnConfirm", new Func<DefaultIdType, decimal, string?, Task>(async (targetVaultId, amount, denominationBreakdown) =>
                {
                    var command = new TransferToVaultCommand
                    {
                        SourceVaultId = sourceVaultId,
                        TargetVaultId = targetVaultId,
                        Amount = amount,
                        DenominationBreakdown = denominationBreakdown
                    };
                    
                    var result = await ApiHelper.ExecuteCallGuardedAsync(
                        () => Client.TransferBetweenVaultsAsync("1", command),
                        successMessage: "Transfer completed successfully.");
                    
                    if (result != null)
                    {
                        var message = $"Transfer of {amount:C} completed.\n" +
                                      $"Source new balance: {result.SourceNewBalance:C}\n" +
                                      $"Target new balance: {result.TargetNewBalance:C}";
                        Snackbar.Add(message, Severity.Success);
                    }
                    
                    await _table.ReloadDataAsync();
                })
            }
        };

        await DialogService.ShowAsync<TransferVaultDialog>("Transfer to Vault", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
