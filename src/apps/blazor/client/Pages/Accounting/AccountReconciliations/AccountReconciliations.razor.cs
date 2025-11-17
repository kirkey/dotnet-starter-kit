namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountReconciliations;

/// <summary>
/// Page for managing account reconciliations.
/// </summary>
public partial class AccountReconciliations
{
    protected EntityServerTableContext<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel> Context { get; set; } = null!;
    private EntityTable<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel> _table = null!;

    private string? ReconciliationStatus { get; set; }
    private string? SubsidiaryLedgerSource { get; set; }
    private bool HasVariance { get; set; }

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel>(
            entityName: "Account Reconciliation",
            entityNamePlural: "Account Reconciliations",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountReconciliationResponse>(x => x.GeneralLedgerAccountId, "GL Account", "GeneralLedgerAccountId"),
                new EntityField<AccountReconciliationResponse>(x => x.ReconciliationDate, "Date", "ReconciliationDate", typeof(DateTime)),
                new EntityField<AccountReconciliationResponse>(x => x.GlBalance, "GL Balance", "GlBalance", typeof(decimal)),
                new EntityField<AccountReconciliationResponse>(x => x.SubsidiaryLedgerBalance, "Sub Balance", "SubsidiaryLedgerBalance", typeof(decimal)),
                new EntityField<AccountReconciliationResponse>(x => x.Variance, "Variance", "Variance", typeof(decimal)),
                new EntityField<AccountReconciliationResponse>(x => x.ReconciliationStatus, "Status", "ReconciliationStatus"),
                new EntityField<AccountReconciliationResponse>(x => x.SubsidiaryLedgerSource, "Source", "SubsidiaryLedgerSource"),
            ],
            enableAdvancedSearch: true,
            idFunc: x => x.Id,
            searchFunc: async filter =>
            {
                var request = new SearchAccountReconciliationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    ReconciliationStatus = ReconciliationStatus,
                    SubsidiaryLedgerSource = SubsidiaryLedgerSource,
                    HasVariance = HasVariance ? true : null
                };
                var result = await Client.SearchAccountReconciliationsEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<AccountReconciliationResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateAccountReconciliationCommand(
                    vm.GeneralLedgerAccountId,
                    vm.AccountingPeriodId,
                    vm.GlBalance,
                    vm.SubsidiaryLedgerBalance,
                    vm.SubsidiaryLedgerSource,
                    vm.ReconciliationDate,
                    vm.VarianceExplanation);
                await Client.CreateAccountReconciliationEndpointAsync("1", command);
                Snackbar.Add("Account reconciliation created successfully", Severity.Success);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateAccountReconciliationCommand(
                    id,
                    vm.GlBalance,
                    vm.SubsidiaryLedgerBalance,
                    vm.VarianceExplanation,
                    vm.LineItemCount,
                    vm.AdjustingEntriesRecorded);
                await Client.UpdateAccountReconciliationEndpointAsync("1", id, command);
                Snackbar.Add("Account reconciliation updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteAccountReconciliationEndpointAsync("1", id);
                Snackbar.Add("Account reconciliation deleted successfully", Severity.Success);
            });

        return Task.CompletedTask;
    }

    private async Task OnViewDetails(DefaultIdType reconciliationId)
    {
        try
        {
            var reconciliation = await Client.GetAccountReconciliationEndpointAsync("1", reconciliationId);
            var parameters = new DialogParameters
            {
                { nameof(AccountReconciliationDetailsDialog.Reconciliation), reconciliation }
            };
            await DialogService.ShowAsync<AccountReconciliationDetailsDialog>("Reconciliation Details", parameters, _dialogOptions);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading details: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnApprove(DefaultIdType reconciliationId)
    {
        var parameters = new DialogParameters
        {
            { nameof(ApproveReconciliationDialog.ReconciliationId), reconciliationId }
        };
        var dialog = await DialogService.ShowAsync<ApproveReconciliationDialog>("Approve Reconciliation", parameters, _dialogOptions);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task OnEdit(DefaultIdType reconciliationId)
    {
        await _table.AddEditModal.EditAsync(reconciliationId);
    }

    private async Task OnDelete(DefaultIdType reconciliationId)
    {
        var confirm = await DialogService.ShowMessageBox(
            "Confirm Delete",
            "Delete this reconciliation? This action cannot be undone.",
            yesText: "Delete", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.DeleteAccountReconciliationEndpointAsync("1", reconciliationId);
                Snackbar.Add("Reconciliation deleted successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error deleting reconciliation: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<AccountReconciliationHelpDialog>("Account Reconciliation Help",
            new DialogParameters(), new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}

