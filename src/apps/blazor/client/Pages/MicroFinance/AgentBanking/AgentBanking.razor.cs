using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.AgentBanking.Dialogs;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.AgentBanking;

/// <summary>
/// Agent Banking page logic. Provides CRUD and workflow operations for agent banking entities.
/// Manages agent network for branchless banking services.
/// </summary>
public partial class AgentBanking
{
    /// <summary>
    /// Table context for the EntityTable component.
    /// </summary>
    protected EntityServerTableContext<AgentBankingResponse, DefaultIdType, AgentBankingViewModel> Context { get; set; } = null!;

    private EntityTable<AgentBankingResponse, DefaultIdType, AgentBankingViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canApprove;
    private bool _canSuspend;
    private bool _canManageFloat;
    private bool _canUpgradeTier;

    private ClientPreference _preference = new();

    // Date picker helper
    private DateTime? _contractStartDate = DateTime.Today;

    // Advanced search filters
    private string? _searchAgentCode;
    private string? SearchAgentCode
    {
        get => _searchAgentCode;
        set
        {
            _searchAgentCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchBusinessName;
    private string? SearchBusinessName
    {
        get => _searchBusinessName;
        set
        {
            _searchBusinessName = value;
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

    private string? _searchTier;
    private string? SearchTier
    {
        get => _searchTier;
        set
        {
            _searchTier = value;
            _ = _table.ReloadDataAsync();
        }
    }

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

        // Check permissions
        var state = await AuthState;
        _canApprove = await AuthService.HasPermissionAsync(state.User, FshPermissions.AgentBankings.Approve);
        _canSuspend = await AuthService.HasPermissionAsync(state.User, FshPermissions.AgentBankings.Suspend);
        _canManageFloat = await AuthService.HasPermissionAsync(state.User, FshPermissions.AgentBankings.ManageFloat);
        _canUpgradeTier = await AuthService.HasPermissionAsync(state.User, FshPermissions.AgentBankings.UpgradeTier);

        Context = new EntityServerTableContext<AgentBankingResponse, DefaultIdType, AgentBankingViewModel>(
            fields:
            [
                new EntityField<AgentBankingResponse>(dto => dto.AgentCode, "Agent Code", "AgentCode"),
                new EntityField<AgentBankingResponse>(dto => dto.BusinessName, "Business Name", "BusinessName"),
                new EntityField<AgentBankingResponse>(dto => dto.ContactName, "Contact", "ContactName"),
                new EntityField<AgentBankingResponse>(dto => dto.PhoneNumber, "Phone", "PhoneNumber"),
                new EntityField<AgentBankingResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<AgentBankingResponse>(dto => dto.Tier, "Tier", "Tier"),
                new EntityField<AgentBankingResponse>(dto => dto.FloatBalance, "Float Balance", "FloatBalance", typeof(decimal)),
                new EntityField<AgentBankingResponse>(dto => dto.IsKycVerified, "KYC Verified", "IsKycVerified", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: () => Task.FromResult(new AgentBankingViewModel
            {
                ContractStartDate = DateOnly.FromDateTime(DateTime.Today)
            }),
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchAgentBankingsCommand>();

                // Apply advanced search filters
                if (!string.IsNullOrWhiteSpace(_searchAgentCode))
                    request = request with { AgentCode = _searchAgentCode };
                if (!string.IsNullOrWhiteSpace(_searchBusinessName))
                    request = request with { BusinessName = _searchBusinessName };
                if (!string.IsNullOrWhiteSpace(_searchStatus))
                    request = request with { Status = _searchStatus };
                if (!string.IsNullOrWhiteSpace(_searchTier))
                    request = request with { Tier = _searchTier };

                var response = await Client.SearchAgentBankingsEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<AgentBankingResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetAgentBankingEndpointAsync("1", id);
                return response.Adapt<AgentBankingViewModel>();
            },
            createFunc: async vm =>
            {
                var command = new CreateAgentBankingCommand
                {
                    AgentCode = vm.AgentCode,
                    BusinessName = vm.BusinessName,
                    ContactName = vm.ContactName,
                    PhoneNumber = vm.PhoneNumber,
                    Address = vm.Address,
                    CommissionRate = vm.CommissionRate,
                    DailyTransactionLimit = vm.DailyTransactionLimit,
                    MonthlyTransactionLimit = vm.MonthlyTransactionLimit,
                    ContractStartDate = vm.ContractStartDate,
                    BranchId = vm.BranchId,
                    Email = vm.Email,
                    GpsCoordinates = vm.GpsCoordinates,
                    OperatingHours = vm.OperatingHours
                };
                var response = await Client.CreateAgentBankingEndpointAsync("1", command);
                return response.Id;
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateAgentBankingCommand
                {
                    Id = id,
                    BusinessName = vm.BusinessName,
                    ContactName = vm.ContactName,
                    PhoneNumber = vm.PhoneNumber,
                    Email = vm.Email,
                    Address = vm.Address,
                    GpsCoordinates = vm.GpsCoordinates,
                    OperatingHours = vm.OperatingHours,
                    CommissionRate = vm.CommissionRate,
                    DailyTransactionLimit = vm.DailyTransactionLimit,
                    MonthlyTransactionLimit = vm.MonthlyTransactionLimit
                };
                await Client.UpdateAgentBankingEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.DeleteAgentBankingEndpointAsync("1", id),
            exportAction: string.Empty,
            entityTypeName: "Agent",
            entityTypeNamePlural: "Agents",
            createPermission: FshPermissions.AgentBankings.Create,
            updatePermission: FshPermissions.AgentBankings.Update,
            deletePermission: FshPermissions.AgentBankings.Delete,
            searchPermission: FshPermissions.AgentBankings.View
        );
    }

    private void OnContractStartDateChanged(AgentBankingViewModel context, DateTime? date)
    {
        _contractStartDate = date;
        if (date.HasValue)
        {
            context.ContractStartDate = DateOnly.FromDateTime(date.Value);
        }
    }

    private async Task ApproveAgent(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Approve Agent",
            "Are you sure you want to approve this agent? They will be able to start processing transactions.",
            yesText: "Approve",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new ApproveAgentBankingCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.ApproveAgentBankingEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Agent approved successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowSuspendDialog(AgentBankingResponse entity)
    {
        var parameters = new DialogParameters<AgentBankingSuspendDialog>
        {
            { x => x.AgentId, entity.Id },
            { x => x.AgentName, entity.BusinessName }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AgentBankingSuspendDialog>("Suspend Agent", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowCreditFloatDialog(AgentBankingResponse entity)
    {
        var parameters = new DialogParameters<AgentBankingFloatDialog>
        {
            { x => x.AgentId, entity.Id },
            { x => x.AgentName, entity.BusinessName },
            { x => x.CurrentBalance, entity.FloatBalance },
            { x => x.IsCredit, true }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AgentBankingFloatDialog>("Credit Float", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowDebitFloatDialog(AgentBankingResponse entity)
    {
        var parameters = new DialogParameters<AgentBankingFloatDialog>
        {
            { x => x.AgentId, entity.Id },
            { x => x.AgentName, entity.BusinessName },
            { x => x.CurrentBalance, entity.FloatBalance },
            { x => x.IsCredit, false }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AgentBankingFloatDialog>("Debit Float", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowUpgradeTierDialog(AgentBankingResponse entity)
    {
        var parameters = new DialogParameters<AgentBankingUpgradeTierDialog>
        {
            { x => x.AgentId, entity.Id },
            { x => x.AgentName, entity.BusinessName },
            { x => x.CurrentTier, entity.Tier }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<AgentBankingUpgradeTierDialog>("Upgrade Tier", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ViewAgentDetails(DefaultIdType id)
    {
        var entity = await Client.GetAgentBankingEndpointAsync("1", id);

        var parameters = new DialogParameters<AgentBankingDetailsDialog>
        {
            { x => x.Entity, entity }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<AgentBankingDetailsDialog>("Agent Details", parameters, options);
    }

    private async Task ShowAgentBankingHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<AgentBankingHelpDialog>("Agent Banking Help", options);
    }
}
