using FSH.Starter.Blazor.Client.Components.Dialogs;
using FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases.Dialogs;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.CollectionCases;

/// <summary>
/// Collection Cases page logic. Provides CRUD and workflow operations for debt collection.
/// </summary>
public partial class CollectionCases
{
    protected EntityServerTableContext<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel> Context { get; set; } = null!;

    private EntityTable<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    // Permission flags
    private bool _canAssign;
    private bool _canRecordContact;
    private bool _canRecordRecovery;
    private bool _canEscalateToLegal;
    private bool _canSettle;
    private bool _canClose;

    private ClientPreference _preference = new();

    // Status filter
    private string? _statusFilter;

    private void OnStatusFilterChanged(string? value)
    {
        _statusFilter = value;
        _ = _table.ReloadDataAsync();
    }

    // Advanced search filters
    private string? _searchCaseNumber;
    private string? SearchCaseNumber
    {
        get => _searchCaseNumber;
        set
        {
            _searchCaseNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchPriority;
    private string? SearchPriority
    {
        get => _searchPriority;
        set
        {
            _searchPriority = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchClassification;
    private string? SearchClassification
    {
        get => _searchClassification;
        set
        {
            _searchClassification = value;
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
        _canAssign = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.Assign);
        _canRecordContact = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.RecordContact);
        _canRecordRecovery = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.RecordRecovery);
        _canEscalateToLegal = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.EscalateToLegal);
        _canSettle = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.Settle);
        _canClose = await AuthService.HasPermissionAsync(state.User, FshPermissions.CollectionCases.Close);

        Context = new EntityServerTableContext<CollectionCaseResponse, DefaultIdType, CollectionCaseViewModel>(
            fields:
            [
                new EntityField<CollectionCaseResponse>(dto => dto.CaseNumber, "Case #", "CaseNumber"),
                new EntityField<CollectionCaseResponse>(dto => dto.Status, "Status", "Status"),
                new EntityField<CollectionCaseResponse>(dto => dto.Priority, "Priority", "Priority"),
                new EntityField<CollectionCaseResponse>(dto => dto.Classification, "Class", "Classification"),
                new EntityField<CollectionCaseResponse>(dto => dto.CurrentDaysPastDue, "DPD", "CurrentDaysPastDue"),
                new EntityField<CollectionCaseResponse>(dto => dto.AmountOverdue, "Overdue", "AmountOverdue", typeof(decimal)),
                new EntityField<CollectionCaseResponse>(dto => dto.TotalOutstanding, "Outstanding", "TotalOutstanding", typeof(decimal)),
                new EntityField<CollectionCaseResponse>(dto => dto.AmountRecovered, "Recovered", "AmountRecovered", typeof(decimal)),
                new EntityField<CollectionCaseResponse>(dto => dto.LastContactDate, "Last Contact", "LastContactDate", typeof(DateOnly)),
            ],
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            getDefaultsFunc: () => Task.FromResult(new CollectionCaseViewModel
            {
                CaseNumber = $"CC-{DateTime.Today:yyyyMMdd}-001",
                DaysPastDue = 30
            }),
            searchFunc: async filter =>
            {
                var request = filter.Adapt<SearchCollectionCasesCommand>();

                if (!string.IsNullOrWhiteSpace(_statusFilter))
                    request = request with { Status = _statusFilter };
                if (!string.IsNullOrWhiteSpace(_searchCaseNumber))
                    request = request with { CaseNumber = _searchCaseNumber };
                if (!string.IsNullOrWhiteSpace(_searchPriority))
                    request = request with { Priority = _searchPriority };
                if (!string.IsNullOrWhiteSpace(_searchClassification))
                    request = request with { Classification = _searchClassification };

                var response = await Client.SearchCollectionCasesEndpointAsync("1", request);
                return response.Adapt<PaginationResponse<CollectionCaseResponse>>();
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.GetCollectionCaseEndpointAsync("1", id);
                return response.Adapt<CollectionCaseViewModel>();
            },
            createFunc: async vm =>
            {
                var command = new CreateCollectionCaseCommand
                {
                    CaseNumber = vm.CaseNumber,
                    LoanId = vm.LoanId,
                    MemberId = vm.MemberId,
                    DaysPastDue = vm.DaysPastDue,
                    AmountOverdue = vm.AmountOverdue,
                    TotalOutstanding = vm.TotalOutstanding
                };
                var response = await Client.CreateCollectionCaseEndpointAsync("1", command);
                return response.Id;
            },
            deleteFunc: async id => await Client.DeleteCollectionCaseEndpointAsync("1", id),
            exportAction: string.Empty,
            entityTypeName: "Collection Case",
            entityTypeNamePlural: "Collection Cases",
            createPermission: FshPermissions.CollectionCases.Create,
            updatePermission: FshPermissions.CollectionCases.Update,
            deletePermission: FshPermissions.CollectionCases.Delete,
            searchPermission: FshPermissions.CollectionCases.View
        );
    }

    private async Task ShowAssignDialog(CollectionCaseResponse entity)
    {
        var parameters = new DialogParameters<CollectionCaseAssignDialog>
        {
            { x => x.CaseId, entity.Id },
            { x => x.CaseNumber, entity.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CollectionCaseAssignDialog>("Assign Collector", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowRecordContactDialog(CollectionCaseResponse entity)
    {
        var parameters = new DialogParameters<CollectionCaseContactDialog>
        {
            { x => x.CaseId, entity.Id },
            { x => x.CaseNumber, entity.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CollectionCaseContactDialog>("Record Contact", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowRecordRecoveryDialog(CollectionCaseResponse entity)
    {
        var parameters = new DialogParameters<CollectionCaseRecoveryDialog>
        {
            { x => x.CaseId, entity.Id },
            { x => x.CaseNumber, entity.CaseNumber },
            { x => x.AmountOverdue, entity.AmountOverdue }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CollectionCaseRecoveryDialog>("Record Recovery", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task EscalateToLegal(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Escalate to Legal",
            "Are you sure you want to escalate this case to legal proceedings? This action cannot be undone.",
            yesText: "Escalate",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new EscalateToLegalCollectionCaseCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.EscalateToLegalCollectionCaseEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Case escalated to legal.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task SettleCase(DefaultIdType id)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Settle Case",
            "Are you sure you want to mark this case as settled?",
            yesText: "Settle",
            cancelText: "Cancel");

        if (confirm == true)
        {
            var command = new SettleCollectionCaseCommand { Id = id };
            await ApiHelper.ExecuteCallGuardedAsync(
                async () => await Client.SettleCollectionCaseEndpointAsync("1", id, command),
                Snackbar,
                successMessage: "Case settled successfully.");
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowCloseDialog(CollectionCaseResponse entity)
    {
        var parameters = new DialogParameters<CollectionCaseCloseDialog>
        {
            { x => x.CaseId, entity.Id },
            { x => x.CaseNumber, entity.CaseNumber }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<CollectionCaseCloseDialog>("Close Case", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ViewCaseDetails(DefaultIdType id)
    {
        var entity = await Client.GetCollectionCaseEndpointAsync("1", id);

        var parameters = new DialogParameters<CollectionCaseDetailsDialog>
        {
            { x => x.Entity, entity }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CollectionCaseDetailsDialog>("Case Details", parameters, options);
    }

    private async Task ShowCollectionCasesHelp()
    {
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        await DialogService.ShowAsync<CollectionCasesHelpDialog>("Collection Cases Help", options);
    }
}
