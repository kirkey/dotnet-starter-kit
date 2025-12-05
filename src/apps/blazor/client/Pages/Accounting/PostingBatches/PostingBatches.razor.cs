namespace FSH.Starter.Blazor.Client.Pages.Accounting.PostingBatches;

public partial class PostingBatches
{
    protected EntityServerTableContext<PostingBatchResponse, DefaultIdType, PostingBatchViewModel> Context { get; set; } = null!;
    private EntityTable<PostingBatchResponse, DefaultIdType, PostingBatchViewModel> _table = null!;

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

    private string? _searchBatchNumber;
    private string? SearchBatchNumber
    {
        get => _searchBatchNumber;
        set
        {
            _searchBatchNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchFromDate;
    private DateTime? SearchFromDate
    {
        get => _searchFromDate;
        set
        {
            _searchFromDate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _searchToDate;
    private DateTime? SearchToDate
    {
        get => _searchToDate;
        set
        {
            _searchToDate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
            _preference = preference;

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<PostingBatchResponse, DefaultIdType, PostingBatchViewModel>(
            entityName: "Posting Batch",
            entityNamePlural: "Posting Batches",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<PostingBatchResponse>(x => x.BatchNumber, "Batch #", "BatchNumber"),
                new EntityField<PostingBatchResponse>(x => x.Status, "Status", "Status"),
                new EntityField<PostingBatchResponse>(x => x.BatchDate, "Batch Date", "BatchDate", typeof(DateTime)),
                new EntityField<PostingBatchResponse>(x => x.Description ?? "-", "Description", "Description")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new PostingBatchSearchQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    Status = SearchStatus,
                    BatchNumber = SearchBatchNumber
                };
                var result = await Client.PostingBatchSearchEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PostingBatchResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePostingBatchCommand
                {
                    BatchNumber = vm.BatchNumber,
                    Description = vm.Description,
                    BatchDate = vm.BatchDate ?? DateTime.Today
                };
                await Client.PostingBatchCreateEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePostingBatchCommand
                {
                    Description = vm.Description
                };
                await Client.PostingBatchUpdateEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.PostingBatchDeleteEndpointAsync("1", id).ConfigureAwait(false);
            },
            getDetailsFunc: async id =>
            {
                var result = await Client.PostingBatchGetEndpointAsync("1", id).ConfigureAwait(false);
                return result.Adapt<PostingBatchViewModel>();
            },
            getDefaultsFunc: () => Task.FromResult(new PostingBatchViewModel
            {
                BatchDate = DateTime.Today,
                Status = "Draft"
            }),
            hasExtraActionsFunc: () => true);

        base.OnInitialized();
    }

    private async Task ViewDetails(DefaultIdType id)
    {
        Snackbar.Add("View details - feature coming soon", Severity.Info);
    }

    private async Task ApproveBatch(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Approve Batch", "Approve this posting batch?", yesText: "Approve", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new ApprovePostingBatchCommand();
                await Client.PostingBatchApproveEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Posting batch approved", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to approve: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task RejectBatch(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Reject Batch", "Reject this posting batch?", yesText: "Reject", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new RejectPostingBatchCommand();
                await Client.PostingBatchRejectEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Posting batch rejected", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reject: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task PostBatch(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Post Batch", "Post this approved batch to the ledger?", yesText: "Post", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                await Client.PostingBatchPostEndpointAsync("1", id).ConfigureAwait(false);
                Snackbar.Add("Posting batch posted", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to post: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ReverseBatch(DefaultIdType id)
    {
        bool? confirmed = await DialogService.ShowMessageBox("Reverse Batch", "Reverse a posted batch? This will generate reversing entries.", yesText: "Reverse", cancelText: "Cancel");
        if (confirmed == true)
        {
            try
            {
                var command = new ReversePostingBatchCommand();
                await Client.PostingBatchReverseEndpointAsync("1", id, command).ConfigureAwait(false);
                Snackbar.Add("Posting batch reversed", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to reverse: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ShowPostingBatchesHelp()
    {
        await DialogService.ShowAsync<PostingBatchesHelpDialog>("Posting Batches Help", new DialogParameters(), new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true });
    }
}
