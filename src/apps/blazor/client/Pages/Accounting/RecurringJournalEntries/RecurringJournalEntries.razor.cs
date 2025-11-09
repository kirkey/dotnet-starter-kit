namespace FSH.Starter.Blazor.Client.Pages.Accounting.RecurringJournalEntries;

public partial class RecurringJournalEntries
{
    // Search filters
    private string? SearchTemplateCode;
    private string? SearchFrequency;
    private string? SearchStatus;
    private bool SearchActiveOnly = true;

    protected EntityServerTableContext<RecurringJournalEntryResponse, DefaultIdType, RecurringJournalEntryViewModel> Context { get; set; } = null!;
    private EntityTable<RecurringJournalEntryResponse, DefaultIdType, RecurringJournalEntryViewModel>? _table;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<RecurringJournalEntryResponse, DefaultIdType, RecurringJournalEntryViewModel>(
            entityName: "Recurring Journal Entry",
            entityNamePlural: "Recurring Journal Entries",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<RecurringJournalEntryResponse>(r => r.TemplateCode, "Template Code", "TemplateCode"),
                new EntityField<RecurringJournalEntryResponse>(r => r.Description, "Description", "Description"),
                new EntityField<RecurringJournalEntryResponse>(r => r.Frequency, "Frequency", "Frequency"),
                new EntityField<RecurringJournalEntryResponse>(r => r.Amount, "Amount", "Amount", typeof(decimal)),
                new EntityField<RecurringJournalEntryResponse>(r => r.StartDate, "Start Date", "StartDate", typeof(DateTime)),
                new EntityField<RecurringJournalEntryResponse>(r => r.NextRunDate, "Next Run", "NextRunDate", typeof(DateTime)),
                new EntityField<RecurringJournalEntryResponse>(r => r.IsActive, "Active", "IsActive", typeof(bool)),
                new EntityField<RecurringJournalEntryResponse>(r => r.Status, "Status", "Status")
            ],
            enableAdvancedSearch: true,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchRecurringJournalEntriesRequest
                {
                    TemplateCode = SearchTemplateCode,
                    Frequency = SearchFrequency,
                    Status = SearchActiveOnly ? "Approved" : SearchStatus,
                    IsActive = SearchActiveOnly ? true : null,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.RecurringJournalEntrySearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<RecurringJournalEntryResponse>>();
            },
            createFunc: async vm => await CreateRecurringJournalEntryAsync(vm),
            updateFunc: async (id, vm) => await UpdateRecurringJournalEntryAsync(id, vm),
            deleteFunc: async id => await DeleteRecurringJournalEntryAsync(id),
            hasExtraActionsFunc: () => true);
        return Task.CompletedTask;
    }

    private async Task CreateRecurringJournalEntryAsync(RecurringJournalEntryViewModel vm)
    {
        var cmd = new CreateRecurringJournalEntryCommand
        {
            TemplateCode = vm.TemplateCode!,
            Description = vm.Description!,
            Frequency = vm.Frequency,
            CustomIntervalDays = vm.CustomIntervalDays,
            Amount = vm.Amount,
            DebitAccountId = vm.DebitAccountId!.Value,
            CreditAccountId = vm.CreditAccountId!.Value,
            StartDate = vm.StartDate!.Value,
            EndDate = vm.EndDate,
            Memo = vm.Memo,
            Notes = vm.Notes
        };
        await Client.RecurringJournalEntryCreateEndpointAsync("1", cmd);
        Snackbar.Add("Recurring journal entry created successfully", Severity.Success);
    }

    private async Task UpdateRecurringJournalEntryAsync(DefaultIdType id, RecurringJournalEntryViewModel vm)
    {
        var cmd = new UpdateRecurringJournalEntryCommand
        {
            Id = id,
            Description = vm.Description,
            EndDate = vm.EndDate,
            Memo = vm.Memo,
            Notes = vm.Notes
        };
        await Client.RecurringJournalEntryUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Recurring journal entry updated successfully", Severity.Success);
    }

    private async Task DeleteRecurringJournalEntryAsync(DefaultIdType id)
    {
        await Client.RecurringJournalEntryDeleteEndpointAsync("1", id);
        Snackbar.Add("Recurring journal entry deleted successfully", Severity.Success);
    }

    private async Task OnApprove(RecurringJournalEntryResponse template)
    {
        var parameters = new DialogParameters
        {
            [nameof(RecurringJournalEntryApproveDialog.TemplateId)] = template.Id,
            [nameof(RecurringJournalEntryApproveDialog.TemplateCode)] = template.TemplateCode
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<RecurringJournalEntryApproveDialog>("Approve Template", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null)
            await _table.ReloadDataAsync();
    }

    private async Task OnGenerate(RecurringJournalEntryResponse template)
    {
        var parameters = new DialogParameters
        {
            [nameof(RecurringJournalEntryGenerateDialog.TemplateId)] = template.Id,
            [nameof(RecurringJournalEntryGenerateDialog.TemplateCode)] = template.TemplateCode
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<RecurringJournalEntryGenerateDialog>("Generate Journal Entry", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null)
            await _table.ReloadDataAsync();
    }

    private async Task OnSuspend(RecurringJournalEntryResponse template)
    {
        var parameters = new DialogParameters
        {
            [nameof(RecurringJournalEntrySuspendDialog.TemplateId)] = template.Id,
            [nameof(RecurringJournalEntrySuspendDialog.TemplateCode)] = template.TemplateCode
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<RecurringJournalEntrySuspendDialog>("Suspend Template", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null)
            await _table.ReloadDataAsync();
    }

    private async Task OnReactivate(RecurringJournalEntryResponse template)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Reactivate Template",
            $"Are you sure you want to reactivate template {template.TemplateCode}?",
            yesText: "Reactivate", cancelText: "Cancel");

        if (confirmed == true)
        {
            try
            {
                await Client.RecurringJournalEntryReactivateEndpointAsync("1", template.Id);
                Snackbar.Add("Template reactivated successfully", Severity.Success);
                if (_table is not null)
                    await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error reactivating template: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnViewDetails(RecurringJournalEntryResponse template)
    {
        var parameters = new DialogParameters { [nameof(RecurringJournalEntryDetailsDialog.Template)] = template };
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<RecurringJournalEntryDetailsDialog>("Template Details", parameters, options);
    }
}

