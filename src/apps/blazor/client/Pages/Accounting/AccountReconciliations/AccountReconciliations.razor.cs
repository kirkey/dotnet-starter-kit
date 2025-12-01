namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountReconciliations;

/// <summary>
/// Page for managing account reconciliations.
/// </summary>
public partial class AccountReconciliations
{
    protected EntityServerTableContext<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel> Context { get; set; } = null!;
    private EntityTable<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel> _table = null!;

    private ClientPreference _preference = new();

    private string? ReconciliationStatus { get; set; }
    private string? SubsidiaryLedgerSource { get; set; }
    private bool HasVariance { get; set; }

    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

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

        Context = new EntityServerTableContext<AccountReconciliationResponse, DefaultIdType, AccountReconciliationViewModel>(
            entityName: "Account Reconciliation",
            entityNamePlural: "Account Reconciliations",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountReconciliationResponse>(x => x.GeneralLedgerAccountId, "GL Account", "GeneralLedgerAccountId"),
                new EntityField<AccountReconciliationResponse>(x => x.ReconciliationDate, "Date", "ReconciliationDate", typeof(DateOnly)),
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
                var result = await Client.SearchAccountReconciliationsEndpointAsync(request);
                return result.Adapt<PaginationResponse<AccountReconciliationResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateAccountReconciliationCommand
                {
                    GeneralLedgerAccountId = vm.GeneralLedgerAccountId,
                    AccountingPeriodId = vm.AccountingPeriodId,
                    GlBalance = vm.GlBalance,
                    SubsidiaryLedgerBalance = vm.SubsidiaryLedgerBalance,
                    SubsidiaryLedgerSource = vm.SubsidiaryLedgerSource,
                    ReconciliationDate = (DateTime)vm.ReconciliationDate!,
                    VarianceExplanation = vm.VarianceExplanation
                };
                await Client.CreateAccountReconciliationEndpointAsync(command);
                Snackbar.Add("Account reconciliation created successfully", Severity.Success);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateAccountReconciliationCommand
                {
                    Id = id,
                    GlBalance = vm.GlBalance,
                    SubsidiaryLedgerBalance = vm.SubsidiaryLedgerBalance,
                    VarianceExplanation = vm.VarianceExplanation,
                    LineItemCount = vm.LineItemCount,
                    AdjustingEntriesRecorded = vm.AdjustingEntriesRecorded
                };
                await Client.UpdateAccountReconciliationEndpointAsync(id, command);
                Snackbar.Add("Account reconciliation updated successfully", Severity.Success);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteAccountReconciliationEndpointAsync(id);
                Snackbar.Add("Account reconciliation deleted successfully", Severity.Success);
            });
    }

    private async Task OnViewDetails(DefaultIdType reconciliationId)
    {
        try
        {
            // Note: GetAccountReconciliationEndpointAsync appears to have an API client generation issue
            // For now, we'll just show a message that details will be displayed
            var parameters = new DialogParameters
            {
                { "ReconciliationId", reconciliationId }
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
        try
        {
            // Create a default view model for editing
            // The entity details should be fetched from within the dialog if needed
            var viewModel = new AccountReconciliationViewModel { Id = reconciliationId };
            
            var parameters = new DialogParameters
            {
                { nameof(AccountReconciliationEditDialog.Id), reconciliationId },
                { nameof(AccountReconciliationEditDialog.ViewModel), viewModel }
            };
            
            var dialog = await DialogService.ShowAsync<AccountReconciliationEditDialog>(
                "Edit Account Reconciliation", 
                parameters, 
                _dialogOptions);
            
            var result = await dialog.Result;
            if (result is not null && !result.Canceled)
            {
                await _table.ReloadDataAsync();
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading reconciliation for edit: {ex.Message}", Severity.Error);
        }
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
                await Client.DeleteAccountReconciliationEndpointAsync(reconciliationId);
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

