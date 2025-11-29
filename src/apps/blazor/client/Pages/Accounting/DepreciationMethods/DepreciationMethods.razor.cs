namespace FSH.Starter.Blazor.Client.Pages.Accounting.DepreciationMethods;

public partial class DepreciationMethods
{
    [Inject] protected ICourier Courier { get; set; } = null!;
    
    private ClientPreference _preference = new();
    // Search filters
    private string? SearchMethodCode;
    private string? SearchMethodName;
    private bool SearchActiveOnly = true;

    protected EntityServerTableContext<DepreciationMethodResponse, DefaultIdType, DepreciationMethodViewModel> Context { get; set; } = null!;
    private EntityTable<DepreciationMethodResponse, DefaultIdType, DepreciationMethodViewModel>? _table;

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

        Context = new EntityServerTableContext<DepreciationMethodResponse, DefaultIdType, DepreciationMethodViewModel>(
            entityName: "Depreciation Method",
            entityNamePlural: "Depreciation Methods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<DepreciationMethodResponse>(m => m.Name, "Method Name", "Name"),
                new EntityField<DepreciationMethodResponse>(m => m.MethodCode, "Code", "MethodCode"),
                new EntityField<DepreciationMethodResponse>(m => m.Description, "Description", "Description"),
                new EntityField<DepreciationMethodResponse>(m => m.Formula, "Formula", "Formula"),
                new EntityField<DepreciationMethodResponse>(m => m.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<DepreciationMethodResponse>(m => m.Status, "Status", "Status")
            ],
            enableAdvancedSearch: true,
            idFunc: m => m.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDepreciationMethodsRequest
                {
                    MethodCode = SearchMethodCode,
                    MethodName = SearchMethodName,
                    IsActive = SearchActiveOnly ? true : null,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.DepreciationMethodSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<DepreciationMethodResponse>>();
            },
            createFunc: async vm => await CreateDepreciationMethodAsync(vm),
            updateFunc: async (id, vm) => await UpdateDepreciationMethodAsync(id, vm),
            deleteFunc: async id => await DeleteDepreciationMethodAsync(id),
            hasExtraActionsFunc: () => true);
    }

    private async Task CreateDepreciationMethodAsync(DepreciationMethodViewModel vm)
    {
        var cmd = new CreateDepreciationMethodRequest
        {
            MethodCode = vm.MethodCode!,
            MethodName = vm.Name!,
            CalculationFormula = vm.Formula,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.DepreciationMethodCreateEndpointAsync("1", cmd);
        Snackbar.Add("Depreciation method created successfully", Severity.Success);
    }

    private async Task UpdateDepreciationMethodAsync(DefaultIdType id, DepreciationMethodViewModel vm)
    {
        var cmd = new UpdateDepreciationMethodRequest
        {
            Id = id,
            MethodName = vm.Name,
            CalculationFormula = vm.Formula,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.DepreciationMethodUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Depreciation method updated successfully", Severity.Success);
    }

    private async Task DeleteDepreciationMethodAsync(DefaultIdType id)
    {
        await Client.DepreciationMethodDeleteEndpointAsync("1", id);
        Snackbar.Add("Depreciation method deleted successfully", Severity.Success);
    }

    private async Task OnActivate(DepreciationMethodResponse method)
    {
        await Client.DepreciationMethodActivateEndpointAsync("1", method.Id);
        Snackbar.Add("Depreciation method activated successfully", Severity.Success);
        if (_table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnDeactivate(DepreciationMethodResponse method)
    {
        await Client.DepreciationMethodDeactivateEndpointAsync("1", method.Id);
        Snackbar.Add("Depreciation method deactivated successfully", Severity.Success);
        if (_table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnViewDetails(DepreciationMethodResponse method)
    {

        var parameters = new DialogParameters { [nameof(DepreciationMethodDetailsDialog.Method)] = method };
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<DepreciationMethodDetailsDialog>("Depreciation Method Details", parameters, options);
    }

    /// <summary>
    /// Show depreciation methods help dialog.
    /// </summary>
    private async Task ShowDepreciationMethodsHelp()
    {
        await DialogService.ShowAsync<DepreciationMethodsHelpDialog>("Depreciation Methods Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

