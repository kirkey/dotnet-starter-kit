namespace FSH.Starter.Blazor.Client.Pages.Accounting.FixedAssets;

public partial class FixedAssets
{
    // Search filters
    private string? SearchAssetName;
    private string? SearchSerialNumber;
    private string? SearchAssetType;
    private string? SearchDepartment;
    private bool SearchOnlyActive;

    protected EntityServerTableContext<FixedAssetResponse, DefaultIdType, FixedAssetViewModel> Context { get; set; } = null!;
    private EntityTable<FixedAssetResponse, DefaultIdType, FixedAssetViewModel>? _table;

    private string UserId => "system"; // TODO: Get from current user context

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<FixedAssetResponse, DefaultIdType, FixedAssetViewModel>(
            entityName: "Fixed Asset",
            entityNamePlural: "Fixed Assets",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<FixedAssetResponse>(a => a.AssetName, "Asset Name", "AssetName"),
                new EntityField<FixedAssetResponse>(a => a.AssetType, "Type", "AssetType"),
                new EntityField<FixedAssetResponse>(a => a.SerialNumber, "Serial #", "SerialNumber"),
                new EntityField<FixedAssetResponse>(a => a.Department, "Department", "Department"),
                new EntityField<FixedAssetResponse>(a => a.PurchaseDate, "Purchase Date", "PurchaseDate", typeof(DateOnly)),
                new EntityField<FixedAssetResponse>(a => a.PurchasePrice, "Purchase Price", "PurchasePrice", typeof(decimal)),
                new EntityField<FixedAssetResponse>(a => a.CurrentBookValue, "Book Value", "CurrentBookValue", typeof(decimal)),
                new EntityField<FixedAssetResponse>(a => a.IsDisposed, "Disposed", "IsDisposed", typeof(bool)),
                new EntityField<FixedAssetResponse>(a => a.Location, "Location", "Location")
            ],
            enableAdvancedSearch: true,
            idFunc: a => a.Id,
            searchFunc: async filter =>
            {
                var request = new SearchFixedAssetsRequest
                {
                    AssetName = SearchAssetName,
                    SerialNumber = SearchSerialNumber,
                    AssetType = SearchAssetType,
                    Department = SearchDepartment,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.FixedAssetSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<FixedAssetResponse>>();
            },
            createFunc: async vm => await CreateFixedAssetAsync(vm),
            updateFunc: async (id, vm) => await UpdateFixedAssetAsync(id, vm),
            deleteFunc: async id => await DeleteFixedAssetAsync(id),
            hasExtraActionsFunc: () => true);
        return Task.CompletedTask;
    }

    private async Task CreateFixedAssetAsync(FixedAssetViewModel vm)
    {
        var cmd = new CreateFixedAssetCommand
        {
            AssetName = vm.AssetName!,
            AssetType = vm.AssetType!,
            PurchaseDate = vm.PurchaseDate!.Value,
            PurchasePrice = vm.PurchasePrice,
            ServiceLife = vm.ServiceLife,
            DepreciationMethodId = vm.DepreciationMethodId!.Value,
            SalvageValue = vm.SalvageValue,
            AccumulatedDepreciationAccountId = vm.AccumulatedDepreciationAccountId!.Value,
            DepreciationExpenseAccountId = vm.DepreciationExpenseAccountId!.Value,
            SerialNumber = vm.SerialNumber,
            Location = vm.Location,
            Department = vm.Department,
            Manufacturer = vm.Manufacturer,
            ModelNumber = vm.ModelNumber,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.FixedAssetCreateEndpointAsync("1", cmd);
        Snackbar.Add("Fixed asset created successfully", Severity.Success);
    }

    private async Task UpdateFixedAssetAsync(DefaultIdType id, FixedAssetViewModel vm)
    {
        var cmd = new UpdateFixedAssetCommand
        {
            Id = id,
            AssetName = vm.AssetName,
            Location = vm.Location,
            Department = vm.Department,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.FixedAssetUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Fixed asset updated successfully", Severity.Success);
    }

    private async Task DeleteFixedAssetAsync(DefaultIdType id)
    {
        await Client.FixedAssetDeleteEndpointAsync("1", id);
        Snackbar.Add("Fixed asset deleted successfully", Severity.Success);
    }

    private async Task OnApprove(FixedAssetResponse asset)
    {
        var cmd = new ApproveFixedAssetCommand
        {
            FixedAssetId = asset.Id,
        };
        await Client.FixedAssetApproveEndpointAsync("1", asset.Id, cmd);
        Snackbar.Add("Fixed asset approved successfully", Severity.Success);
        if (_table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnReject(FixedAssetResponse asset)
    {
        var dialog = await DialogService.ShowAsync<FixedAssetRejectDialog>("Reject Fixed Asset", new DialogParameters { ["FixedAssetId"] = asset.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnDepreciate(FixedAssetResponse asset)
    {
        var dialog = await DialogService.ShowAsync<FixedAssetDepreciateDialog>("Record Depreciation", new DialogParameters { ["FixedAssetId"] = asset.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnUpdateMaintenance(FixedAssetResponse asset)
    {
        var dialog = await DialogService.ShowAsync<FixedAssetMaintenanceDialog>("Update Maintenance", new DialogParameters { ["FixedAssetId"] = asset.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnDispose(FixedAssetResponse asset)
    {
        var dialog = await DialogService.ShowAsync<FixedAssetDisposeDialog>("Dispose Asset", new DialogParameters { ["FixedAssetId"] = asset.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters { [nameof(FixedAssetDetailsDialog.FixedAssetId)] = id };
        var options = new DialogOptions { MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<FixedAssetDetailsDialog>("Fixed Asset Details", parameters, options);
    }

    /// <summary>
    /// Show fixed assets help dialog.
    /// </summary>
    private async Task ShowFixedAssetsHelp()
    {
        await DialogService.ShowAsync<FixedAssetsHelpDialog>("Fixed Assets Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

