namespace FSH.Starter.Blazor.Client.Pages.Accounting.Checks;

public partial class Checks
{
    protected EntityServerTableContext<CheckSearchResponse, DefaultIdType, CheckViewModel> Context { get; set; } = null!;

    private EntityTable<CheckSearchResponse, DefaultIdType, CheckViewModel> _table = null!;

    /// <summary>
    /// Client UI preferences for styling.
    /// </summary>
    private ClientPreference _preference = new();

    // Dialog visibility flags
    private bool _issueDialogVisible;
    private bool _voidDialogVisible;
    private bool _clearDialogVisible;
    private bool _stopPaymentDialogVisible;

    // Dialog options
    private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    // Command objects for dialogs
    private IssueCheckCommand _issueCommand = new()
    {
        CheckId = DefaultIdType.Empty,
        Amount = 0,
        PayeeName = string.Empty,
        IssuedDate = DateTime.UtcNow,
        PayeeId = null,
        VendorId = null,
        PaymentId = null,
        ExpenseId = null,
        Memo = null
    };

    private VoidCheckCommand _voidCommand = new()
    {
        CheckId = DefaultIdType.Empty,
        VoidReason = string.Empty,
        VoidedDate = DateTime.UtcNow
    };

    private ClearCheckCommand _clearCommand = new()
    {
        CheckId = DefaultIdType.Empty,
        ClearedDate = DateTime.UtcNow
    };

    private StopPaymentCheckCommand _stopPaymentCommand = new()
    {
        CheckId = DefaultIdType.Empty,
        StopPaymentReason = string.Empty,
        StopPaymentDate = DateTime.UtcNow
    };

    private PrintCheckCommand _printCommand = new()
    {
        CheckId = DefaultIdType.Empty,
        PrintedBy = "System",
        PrintedDate = DateTime.UtcNow
    };

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

        Context = new EntityServerTableContext<CheckSearchResponse, DefaultIdType, CheckViewModel>(
            entityName: "Check",
            entityNamePlural: "Checks",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<CheckSearchResponse>(response => response.CheckNumber, "Check Number", "CheckNumber"),
                new EntityField<CheckSearchResponse>(response => response.BankAccountCode, "Account", "BankAccountCode"),
                new EntityField<CheckSearchResponse>(response => response.BankAccountName, "Account Name", "BankAccountName"),
                new EntityField<CheckSearchResponse>(response => response.BankName, "Bank", "BankName"),
                new EntityField<CheckSearchResponse>(response => response.Status, "Status", "Status"),
                new EntityField<CheckSearchResponse>(response => response.Amount, "Amount", "Amount", typeof(decimal?)),
                new EntityField<CheckSearchResponse>(response => response.PayeeName, "Payee", "PayeeName"),
                new EntityField<CheckSearchResponse>(response => response.IssuedDate, "Issued Date", "IssuedDate", typeof(DateOnly?)),
                new EntityField<CheckSearchResponse>(response => response.ClearedDate, "Cleared Date", "ClearedDate", typeof(DateOnly?)),
                new EntityField<CheckSearchResponse>(response => response.IsPrinted, "Printed", "IsPrinted", typeof(bool)),
                new EntityField<CheckSearchResponse>(response => response.IsStopPayment, "Stop Payment", "IsStopPayment", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new CheckSearchQuery
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.CheckSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<CheckSearchResponse>>();
            },
            createFunc: async check =>
            {
                // For bundle creation, use StartCheckNumber and EndCheckNumber
                if (!string.IsNullOrWhiteSpace(check.StartCheckNumber) && !string.IsNullOrWhiteSpace(check.EndCheckNumber))
                {
                    var createCommand = check.Adapt<CreateCheckCommand>();
                    await Client.CheckCreateEndpointAsync("1", createCommand);
                    Snackbar.Add($"Check bundle created: {check.StartCheckNumber} to {check.EndCheckNumber}", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Please provide both Start and End check numbers for bundle creation.", Severity.Warning);
                }
            },
            updateFunc: async (id, check) =>
            {
                // Update available checks with new command
                var updateCommand = check.Adapt<UpdateCheckCommand>();
                updateCommand.CheckId = id;
                await Client.CheckUpdateEndpointAsync("1", id, updateCommand);
            },
            deleteFunc: null,
            // getDetailsFunc: async id =>
            // {
            //     var response = await Client.CheckGetEndpointAsync("1", id);
            //     return response.Adapt<CheckViewModel>();
            // },
            hasExtraActionsFunc: () => true);

        await Task.CompletedTask;
    }

    // Issue Check Dialog
    private void OnIssueCheck(DefaultIdType checkId)
    {
        _issueCommand = new()
        {
            CheckId = checkId,
            Amount = 0,
            PayeeName = string.Empty,
            IssuedDate = DateTime.UtcNow,
            PayeeId = null,
            VendorId = null,
            PaymentId = null,
            ExpenseId = null,
            Memo = null
        };
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

            await Client.CheckIssueEndpointAsync("1", _issueCommand);
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
        _voidCommand = new()
        {
            CheckId = checkId,
            VoidReason = string.Empty,
            VoidedDate = DateTime.UtcNow
        };
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

            await Client.CheckVoidEndpointAsync("1", _voidCommand);
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
        _clearCommand = new()
        {
            CheckId = checkId,
            ClearedDate = DateTime.UtcNow
        };
        _clearDialogVisible = true;
    }

    private async Task SubmitClearCheck()
    {
        try
        {
            await Client.CheckClearEndpointAsync("1", _clearCommand);
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
        _stopPaymentCommand = new()
        {
            CheckId = checkId,
            StopPaymentReason = string.Empty,
            StopPaymentDate = DateTime.UtcNow
        };
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

            await Client.CheckStopPaymentEndpointAsync("1", _stopPaymentCommand);
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

            _printCommand.CheckId = checkId;
            _printCommand.PrintedBy = printedBy;
            _printCommand.PrintedDate = DateTime.UtcNow;
            
            await Client.CheckPrintEndpointAsync("1", _printCommand);
            
            Snackbar.Add("Check marked as printed.", Severity.Success);
            await _table.ReloadDataAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error marking check as printed: {ex.Message}", Severity.Error);
        }
    }

    // Advanced Search Properties
    private string? _checkNumber;
    public string? CheckNumber
    {
        get => _checkNumber;
        set
        {
            _checkNumber = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _bankAccountCode;
    public string? BankAccountCode
    {
        get => _bankAccountCode;
        set
        {
            _bankAccountCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _status;
    public string? Status
    {
        get => _status;
        set
        {
            _status = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _payeeName;
    public string? PayeeName
    {
        get => _payeeName;
        set
        {
            _payeeName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal? _amountFrom;
    public decimal? AmountFrom
    {
        get => _amountFrom;
        set
        {
            _amountFrom = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal? _amountTo;
    public decimal? AmountTo
    {
        get => _amountTo;
        set
        {
            _amountTo = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _issuedDateFrom;
    public DateTime? IssuedDateFrom
    {
        get => _issuedDateFrom;
        set
        {
            _issuedDateFrom = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DateTime? _issuedDateTo;
    public DateTime? IssuedDateTo
    {
        get => _issuedDateTo;
        set
        {
            _issuedDateTo = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _isPrinted;
    public bool? IsPrinted
    {
        get => _isPrinted;
        set
        {
            _isPrinted = value;
            _ = _table.ReloadDataAsync();
        }
    }

    /// <summary>
    /// Show checks help dialog.
    /// </summary>
    private async Task ShowChecksHelp()
    {
        await DialogService.ShowAsync<ChecksHelpDialog>("Check Management Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}
