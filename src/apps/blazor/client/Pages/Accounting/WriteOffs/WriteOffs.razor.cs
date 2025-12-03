namespace FSH.Starter.Blazor.Client.Pages.Accounting.WriteOffs;

public partial class WriteOffs
{
    // Search filters
    private string? SearchReferenceNumber;
    private string? SearchWriteOffType;
    private string? SearchApprovalStatus;
    private string? SearchStatus;
    private bool SearchOnlyRecovered;

    protected EntityServerTableContext<WriteOffResponse, DefaultIdType, WriteOffViewModel> Context { get; set; } = null!;
    private EntityTable<WriteOffResponse, DefaultIdType, WriteOffViewModel>? _table;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    private string UserId => "system"; // TODO: Get from current user context

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

        Context = new EntityServerTableContext<WriteOffResponse, DefaultIdType, WriteOffViewModel>(
            entityName: "Write-Off",
            entityNamePlural: "Write-Offs",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<WriteOffResponse>(w => w.ReferenceNumber, "Reference #", "ReferenceNumber"),
                new EntityField<WriteOffResponse>(w => w.WriteOffDate, "Date", "WriteOffDate", typeof(DateOnly)),
                new EntityField<WriteOffResponse>(w => w.WriteOffType, "Type", "WriteOffType"),
                new EntityField<WriteOffResponse>(w => w.CustomerName, "Customer", "CustomerName"),
                new EntityField<WriteOffResponse>(w => w.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<WriteOffResponse>(w => w.RecoveredAmount, "Recovered", "RecoveredAmount", typeof(decimal)),
                new EntityField<WriteOffResponse>(w => w.Status, "Status", "Status"),
                new EntityField<WriteOffResponse>(w => w.ApprovalStatus, "Approval", "ApprovalStatus"),
                new EntityField<WriteOffResponse>(w => w.IsRecovered, "Recovered", "IsRecovered", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: w => w.Id,
            searchFunc: async filter =>
            {
                var request = new SearchWriteOffsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    ReferenceNumber = SearchReferenceNumber,
                    CustomerId = null,
                    WriteOffType = SearchWriteOffType,
                    Status = SearchStatus,
                    IsRecovered = SearchOnlyRecovered ? true : null
                };
                var result = await Client.WriteOffSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<WriteOffResponse>>();
            },
            createFunc: async vm => await CreateWriteOffAsync(vm),
            updateFunc: async (id, vm) => await UpdateWriteOffAsync(id, vm),
            deleteFunc: null,
            hasExtraActionsFunc: () => true);
         await Task.CompletedTask;
    }

    private async Task CreateWriteOffAsync(WriteOffViewModel vm)
    {
        var cmd = new WriteOffCreateCommand
        {
            ReferenceNumber = vm.ReferenceNumber!,
            WriteOffDate = vm.WriteOffDate!.Value,
            WriteOffType = vm.WriteOffType!,
            Amount = vm.Amount,
            CustomerId = vm.CustomerId,
            InvoiceId = vm.InvoiceId,
            ReceivableAccountId = vm.ReceivableAccountId!.Value,
            ExpenseAccountId = vm.ExpenseAccountId!.Value,
            Reason = vm.Reason,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.WriteOffCreateEndpointAsync("1", cmd);
        Snackbar.Add("Write-off created successfully", Severity.Success);
    }

    private async Task UpdateWriteOffAsync(DefaultIdType id, WriteOffViewModel vm)
    {
        var cmd = new UpdateWriteOffCommand
        {
            Id = id,
            Reason = vm.Reason,
            Description = vm.Description,
            Notes = vm.Notes
        };
        await Client.WriteOffUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Write-off updated successfully", Severity.Success);
    }

    private async Task OnApprove(WriteOffResponse w)
    {
        var cmd = new ApproveWriteOffCommand
        {
            Id = w.Id,
        };
        await Client.WriteOffApproveEndpointAsync("1", w.Id, cmd);
        Snackbar.Add("Write-off approved successfully", Severity.Success);
        if (_table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnViewDetails(DefaultIdType id)
    {
        var parameters = new DialogParameters { [nameof(WriteOffDetailsDialog.WriteOffId)] = id };
        var options = new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<WriteOffDetailsDialog>("Write-Off Details", parameters, options);
    }

    private async Task OnReject(WriteOffResponse w)
    {
        var dialog = await DialogService.ShowAsync<WriteOffRejectDialog>("Reject Write-Off", new DialogParameters { ["WriteOffId"] = w.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnPost(WriteOffResponse w)
    {
        var dialog = await DialogService.ShowAsync<WriteOffPostDialog>("Post Write-Off", new DialogParameters { ["WriteOffId"] = w.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnRecordRecovery(WriteOffResponse w)
    {
        var dialog = await DialogService.ShowAsync<WriteOffRecordRecoveryDialog>("Record Recovery", parameters: new DialogParameters { ["WriteOffId"] = w.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    private async Task OnReverse(WriteOffResponse w)
    {
        var dialog = await DialogService.ShowAsync<WriteOffReverseDialog>("Reverse Write-Off", new DialogParameters { ["WriteOffId"] = w.Id });
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) await _table.ReloadDataAsync();
    }

    /// <summary>
    /// Show write-offs help dialog.
    /// </summary>
    private async Task ShowWriteOffsHelp()
    {
        await DialogService.ShowAsync<WriteOffsHelpDialog>("Write-Offs Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

