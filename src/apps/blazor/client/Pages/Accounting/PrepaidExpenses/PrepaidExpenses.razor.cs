namespace FSH.Starter.Blazor.Client.Pages.Accounting.PrepaidExpenses;

public partial class PrepaidExpenses
{
    // Search filters
    private string? SearchNumber;
    private string? SearchStatus;
    private bool SearchActiveOnly = true;
    private DateTime? SearchStartDateFrom;
    private DateTime? SearchStartDateTo;

    protected EntityServerTableContext<PrepaidExpenseResponse, DefaultIdType, PrepaidExpenseViewModel> Context { get; set; } = null!;
    private EntityTable<PrepaidExpenseResponse, DefaultIdType, PrepaidExpenseViewModel>? _table;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<PrepaidExpenseResponse, DefaultIdType, PrepaidExpenseViewModel>(
            entityName: "Prepaid Expense",
            entityNamePlural: "Prepaid Expenses",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<PrepaidExpenseResponse>(e => e.PrepaidNumber, "Prepaid Number", "PrepaidNumber"),
                new EntityField<PrepaidExpenseResponse>(e => e.Description, "Description", "Description"),
                new EntityField<PrepaidExpenseResponse>(e => e.TotalAmount, "Total Amount", "TotalAmount", typeof(decimal)),
                new EntityField<PrepaidExpenseResponse>(e => e.AmortizedAmount, "Amortized", "AmortizedAmount", typeof(decimal)),
                new EntityField<PrepaidExpenseResponse>(e => e.RemainingAmount, "Remaining", "RemainingAmount", typeof(decimal)),
                new EntityField<PrepaidExpenseResponse>(e => e.StartDate, "Start Date", "StartDate", typeof(DateOnly)),
                new EntityField<PrepaidExpenseResponse>(e => e.EndDate, "End Date", "EndDate", typeof(DateOnly)),
                new EntityField<PrepaidExpenseResponse>(e => e.Status, "Status", "Status"),
                new EntityField<PrepaidExpenseResponse>(e => e.IsFullyAmortized, "Fully Amortized", "IsFullyAmortized", typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: e => e.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPrepaidExpensesRequest
                {
                    PrepaidNumber = SearchNumber,
                    Status = SearchActiveOnly ? "Active" : SearchStatus,
                    StartDateFrom = SearchStartDateFrom,
                    StartDateTo = SearchStartDateTo,
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.PrepaidExpenseSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<PrepaidExpenseResponse>>();
            },
            createFunc: async vm => await CreatePrepaidExpenseAsync(vm),
            updateFunc: async (id, vm) => await UpdatePrepaidExpenseAsync(id, vm),
            hasExtraActionsFunc: () => true);
        return Task.CompletedTask;
    }

    private async Task CreatePrepaidExpenseAsync(PrepaidExpenseViewModel vm)
    {
        var cmd = new PrepaidExpenseCreateCommand
        {
            PrepaidNumber = vm.PrepaidNumber!,
            Description = vm.Description!,
            TotalAmount = vm.TotalAmount,
            StartDate = vm.StartDate!.Value,
            EndDate = vm.EndDate!.Value,
            PrepaidAssetAccountId = vm.PrepaidAssetAccountId!.Value,
            ExpenseAccountId = vm.ExpenseAccountId!.Value,
            PaymentDate = vm.PaymentDate!.Value,
            AmortizationSchedule = vm.AmortizationSchedule,
            VendorId = vm.VendorId,
            VendorName = vm.VendorName,
            PaymentId = vm.PaymentId,
            CostCenterId = null,
            PeriodId = null,
            Notes = vm.Notes
        };
        await Client.PrepaidExpenseCreateEndpointAsync("1", cmd);
        Snackbar.Add("Prepaid expense created successfully", Severity.Success);
    }

    private async Task UpdatePrepaidExpenseAsync(DefaultIdType id, PrepaidExpenseViewModel vm)
    {
        var cmd = new UpdatePrepaidExpenseCommand
        {
            Id = id,
            Description = vm.Description,
            EndDate = vm.EndDate,
            CostCenterId = null,
            Notes = vm.Notes
        };
        await Client.PrepaidExpenseUpdateEndpointAsync("1", id, cmd);
        Snackbar.Add("Prepaid expense updated successfully", Severity.Success);
    }

    private async Task OnAmortize(PrepaidExpenseResponse expense)
    {
        var parameters = new DialogParameters
        {
            [nameof(PrepaidExpenseAmortizeDialog.PrepaidExpenseId)] = expense.Id,
            [nameof(PrepaidExpenseAmortizeDialog.PrepaidNumber)] = expense.PrepaidNumber,
            [nameof(PrepaidExpenseAmortizeDialog.RemainingAmount)] = expense.RemainingAmount
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<PrepaidExpenseAmortizeDialog>("Record Amortization", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) 
            await _table.ReloadDataAsync();
    }

    private async Task OnClose(PrepaidExpenseResponse expense)
    {
        var parameters = new DialogParameters
        {
            [nameof(PrepaidExpenseCloseDialog.PrepaidExpenseId)] = expense.Id,
            [nameof(PrepaidExpenseCloseDialog.PrepaidNumber)] = expense.PrepaidNumber
        };
        var options = new DialogOptions { MaxWidth = MaxWidth.Small, FullWidth = true, CloseButton = true };
        var dialog = await DialogService.ShowAsync<PrepaidExpenseCloseDialog>("Close Prepaid Expense", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && _table is not null) 
            await _table.ReloadDataAsync();
    }

    private async Task OnCancel(PrepaidExpenseResponse expense)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Cancel Prepaid Expense",
            $"Are you sure you want to cancel prepaid expense {expense.PrepaidNumber}? This action cannot be undone.",
            yesText: "Cancel Expense", cancelText: "Keep");

        if (confirmed == true)
        {
            try
            {
                var cmd = new CancelPrepaidExpenseCommand { Id = expense.Id };
                await Client.PrepaidExpenseCancelEndpointAsync("1", expense.Id, cmd);
                Snackbar.Add("Prepaid expense cancelled successfully", Severity.Success);
                if (_table is not null) 
                    await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error cancelling prepaid expense: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnViewDetails(PrepaidExpenseResponse expense)
    {
        var parameters = new DialogParameters { [nameof(PrepaidExpenseDetailsDialog.Expense)] = expense };
        var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true, CloseOnEscapeKey = true };
        await DialogService.ShowAsync<PrepaidExpenseDetailsDialog>("Prepaid Expense Details", parameters, options);
    }

    /// <summary>
    /// Show prepaid expenses help dialog.
    /// </summary>
    private async Task ShowPrepaidExpensesHelp()
    {
        await DialogService.ShowAsync<PrepaidExpensesHelpDialog>("Prepaid Expenses Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

