namespace FSH.Starter.Blazor.Client.Pages.Accounting.Checks;

public partial class Checks
{
    protected EntityServerTableContext<CheckSearchResponse, DefaultIdType, CheckViewModel> Context { get; set; } = default!;

    private EntityTable<CheckSearchResponse, DefaultIdType, CheckViewModel> _table = default!;

    // Dialog visibility flags
    private bool _issueDialogVisible;
    private bool _voidDialogVisible;
    private bool _clearDialogVisible;
    private bool _stopPaymentDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs
    private CheckIssueCommand _issueCommand = new() { CheckId = Guid.Empty, Amount = 0, PayeeName = string.Empty, IssuedDate = DateTime.UtcNow };
    private CheckVoidCommand _voidCommand = new() { CheckId = Guid.Empty, VoidReason = string.Empty };
    private CheckClearCommand _clearCommand = new() { CheckId = Guid.Empty, ClearedDate = DateTime.UtcNow };
    private CheckStopPaymentCommand _stopPaymentCommand = new() { CheckId = Guid.Empty, StopPaymentReason = string.Empty };
    private CheckPrintCommand _printCommand = new() { CheckId = Guid.Empty, PrintedBy = string.Empty };

    // Get status color for badges
    private static Color GetStatusColor(string? status) => status switch
    {
        "Available" => Color.Info,
        "Issued" => Color.Primary,
        "Cleared" => Color.Success,
        "Void" => Color.Error,
        "Stale" => Color.Warning,
        "StopPayment" => Color.Warning,
        _ => Color.Default
    };

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<CheckSearchResponse, DefaultIdType, CheckViewModel>(
            entityName: "Check",
            entityNamePlural: "Checks",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<CheckSearchResponse>(response => response.CheckNumber, "Check Number", "CheckNumber"),
                new EntityField<CheckSearchResponse>(response => response.BankAccountCode, "Account", "BankAccountCode"),
                new EntityField<CheckSearchResponse>(response => response.BankAccountName, "Account Name", "BankAccountName"),
                new EntityField<CheckSearchResponse>(response => response.Status, "Status", "Status"),
                new EntityField<CheckSearchResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal?)),
                new EntityField<CheckSearchResponse>(response => response.PayeeName, "Payee", "PayeeName"),
                new EntityField<CheckSearchResponse>(response => response.IssuedDate, "Issued Date", "IssuedDate", typeof(DateTime?)),
                new EntityField<CheckSearchResponse>(response => response.ClearedDate, "Cleared Date", "ClearedDate", typeof(DateTime?)),
                new EntityField<CheckSearchResponse>(response => response.IsPrinted, "Printed", "IsPrinted", typeof(bool)),
                new EntityField<CheckSearchResponse>(response => response.IsStopPayment, "Stop Payment", "IsStopPayment", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<CheckSearchQuery>();
                var result = await Client.CheckSearchEndpointAsync(paginationFilter);
                return result.Adapt<PaginationResponse<CheckSearchResponse>>();
            },
            createFunc: async check =>
            {
                await Client.CheckCreateEndpointAsync(check.Adapt<CheckCreateCommand>());
            },
            updateFunc: async (id, check) =>
            {
                // Checks don't have a traditional update endpoint - use view-only mode
                // Updates happen through specialized operations (issue, void, clear, etc.)
                await Task.CompletedTask;
            },
            deleteFunc: async id =>
            {
                // Only allow deletion of Available checks
                var checkDetails = await Client.CheckGetEndpointAsync(id);
                if (checkDetails.Status != "Available")
                {
                    Snackbar.Add("Only available checks can be deleted.", Severity.Warning);
                    return;
                }
                // Note: There's no delete endpoint in the current API - checks are voided instead
                // This would need to be implemented if deletion is required
                Snackbar.Add("Check deletion not implemented. Use Void instead for issued checks.", Severity.Info);
            },
            getDetailsFunc: async id =>
            {
                var response = await Client.CheckGetEndpointAsync(id);
                return response.Adapt<CheckViewModel>();
            },
            hasExtraActionsFunc: () => true);

        return Task.CompletedTask;
    }

    // Issue Check Dialog
    private void OnIssueCheck(DefaultIdType checkId)
    {
        _issueCommand = new CheckIssueCommand { CheckId = checkId, Amount = 0, PayeeName = string.Empty, IssuedDate = DateTime.UtcNow };
        _issueDialogVisible = true;
    }

    private async Task SubmitIssueCheck()
    {
        try
        {
            if (_issueCommand.Amount <= 0)
            {
                Snackbar.Add("Amount must be greater than zero.", Severity.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_issueCommand.PayeeName))
            {
                Snackbar.Add("Payee name is required.", Severity.Warning);
                return;
            }

            await Client.CheckIssueEndpointAsync(_issueCommand);
            Snackbar.Add("Check issued successfully.", Severity.Success);
            _issueDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error issuing check: {ex.Message}", Severity.Error);
        }
    }

    // Void Check Dialog
    private void OnVoidCheck(DefaultIdType checkId)
    {
        _voidCommand = new CheckVoidCommand { CheckId = checkId, VoidReason = string.Empty };
        _voidDialogVisible = true;
    }

    private async Task SubmitVoidCheck()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_voidCommand.VoidReason) || _voidCommand.VoidReason.Length < 5)
            {
                Snackbar.Add("Void reason must be at least 5 characters.", Severity.Warning);
                return;
            }

            await Client.CheckVoidEndpointAsync(_voidCommand);
            Snackbar.Add("Check voided successfully.", Severity.Success);
            _voidDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error voiding check: {ex.Message}", Severity.Error);
        }
    }

    // Clear Check Dialog
    private void OnClearCheck(DefaultIdType checkId)
    {
        _clearCommand = new CheckClearCommand { CheckId = checkId, ClearedDate = DateTime.UtcNow };
        _clearDialogVisible = true;
    }

    private async Task SubmitClearCheck()
    {
        try
        {
            await Client.CheckClearEndpointAsync(_clearCommand);
            Snackbar.Add("Check marked as cleared.", Severity.Success);
            _clearDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error clearing check: {ex.Message}", Severity.Error);
        }
    }

    // Stop Payment Dialog
    private void OnStopPayment(DefaultIdType checkId)
    {
        _stopPaymentCommand = new CheckStopPaymentCommand { CheckId = checkId, StopPaymentReason = string.Empty };
        _stopPaymentDialogVisible = true;
    }

    private async Task SubmitStopPayment()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_stopPaymentCommand.StopPaymentReason) || _stopPaymentCommand.StopPaymentReason.Length < 10)
            {
                Snackbar.Add("Stop payment reason must be at least 10 characters.", Severity.Warning);
                return;
            }

            await Client.CheckStopPaymentEndpointAsync(_stopPaymentCommand);
            Snackbar.Add("Stop payment request submitted successfully.", Severity.Success);
            _stopPaymentDialogVisible = false;
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error submitting stop payment: {ex.Message}", Severity.Error);
        }
    }

    // Print Check - Direct action without dialog
    private async Task OnPrintCheck(DefaultIdType checkId)
    {
        try
        {
            // Use a default value or get from current context
            var printedBy = "System"; // This should ideally come from the authenticated user context

            _printCommand = new CheckPrintCommand { CheckId = checkId, PrintedBy = printedBy, PrintedDate = DateTime.UtcNow };
            await Client.CheckPrintEndpointAsync(_printCommand);
            
            Snackbar.Add("Check marked as printed.", Severity.Success);
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error marking check as printed: {ex.Message}", Severity.Error);
        }
    }
}
