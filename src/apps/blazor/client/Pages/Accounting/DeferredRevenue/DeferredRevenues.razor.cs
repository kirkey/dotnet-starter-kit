namespace FSH.Starter.Blazor.Client.Pages.Accounting.DeferredRevenue;

public partial class DeferredRevenues
{
    // Search filters
    private string? SearchNumber;
    private bool SearchUnrecognizedOnly = true;
    private DateTime? SearchDateFrom;
    private DateTime? SearchDateTo;

    protected EntityServerTableContext<DeferredRevenueResponse, DefaultIdType, DeferredRevenueViewModel> Context { get; set; } = null!;
    private EntityTable<DeferredRevenueResponse, DefaultIdType, DeferredRevenueViewModel>? _table;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<DeferredRevenueResponse, DefaultIdType, DeferredRevenueViewModel>(
            entityName: "Deferred Revenue",
            entityNamePlural: "Deferred Revenues",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<DeferredRevenueResponse>(d => d.DeferredRevenueNumber, "Revenue Number", "DeferredRevenueNumber"),
                new EntityField<DeferredRevenueResponse>(d => d.RecognitionDate, "Recognition Date", "RecognitionDate", typeof(DateTime)),
                new EntityField<DeferredRevenueResponse>(d => d.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<DeferredRevenueResponse>(d => d.Description, "Description", "Description"),
                new EntityField<DeferredRevenueResponse>(d => d.IsRecognized, "Recognized", "IsRecognized", typeof(bool)),
                new EntityField<DeferredRevenueResponse>(d => d.RecognizedDate, "Recognized Date", "RecognizedDate", typeof(DateTime?))
            ],
            enableAdvancedSearch: true,
            idFunc: d => d.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDeferredRevenuesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    DeferredRevenueNumber = SearchNumber,
                    IsRecognized = SearchUnrecognizedOnly ? false : null,
                    RecognitionDateFrom = SearchDateFrom,
                    RecognitionDateTo = SearchDateTo,
                };
                var result = await Client.DeferredRevenueSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<DeferredRevenueResponse>>();
            },
            createFunc: async vm => await CreateDeferredRevenueAsync(vm),
            updateFunc: async (id, vm) => await UpdateDeferredRevenueAsync(id, vm),
            deleteFunc: async id => await DeleteDeferredRevenueAsync(id),
            hasExtraActionsFunc: () => true);
        return Task.CompletedTask;
    }

    private async Task CreateDeferredRevenueAsync(DeferredRevenueViewModel vm)
    {
        var cmd = new CreateDeferredRevenueCommand()
        {
            DeferredRevenueNumber = vm.DeferredRevenueNumber!,
            RecognitionDate = vm.RecognitionDate!.Value,
            Amount = vm.Amount,
            Description = vm.Description
        };
        await Client.DeferredRevenueCreateEndpointAsync("1", cmd);
        Snackbar.Add("Deferred revenue created successfully", Severity.Success);
    }

    private async Task UpdateDeferredRevenueAsync(DefaultIdType id, DeferredRevenueViewModel vm)
    {
        var cmd = new UpdateDeferredRevenueCommand
        {
            Id = id,
            RecognitionDate = vm.RecognitionDate,
            Amount = vm.Amount,
            Description = vm.Description
       };
        await Client.DeferredRevenueUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Deferred revenue updated successfully", Severity.Success);
    }

    private async Task DeleteDeferredRevenueAsync(DefaultIdType id)
    {
        await Client.DeferredRevenueDeleteEndpointAsync("1", id);
        Snackbar.Add("Deferred revenue deleted successfully", Severity.Success);
    }

    private async Task OnRecognize(DeferredRevenueResponse revenue)
    {
        var parameters = new DialogParameters
        {
            [nameof(DeferredRevenueRecognizeDialog.DeferredRevenueId)] = revenue.Id,
            [nameof(DeferredRevenueRecognizeDialog.DeferredRevenueNumber)] = revenue.DeferredRevenueNumber
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<DeferredRevenueRecognizeDialog>("Recognize Revenue", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) 
            await _table.ReloadDataAsync();
    }

    private async Task OnViewDetails(DeferredRevenueResponse revenue)
    {
        var parameters = new DialogParameters { [nameof(DeferredRevenueDetailsDialog.Revenue)] = revenue };
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<DeferredRevenueDetailsDialog>("Deferred Revenue Details", parameters, options);
    }
}

