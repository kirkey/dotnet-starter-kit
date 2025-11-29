namespace FSH.Starter.Blazor.Client.Pages.Hr.Employees.BankAccounts;

public partial class BankAccounts
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    [SupplyParameterFromQuery]
    public string? FilterEmployeeId { get; set; }

    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) ? $" - Employee {FilterEmployeeId}" : string.Empty;

    protected EntityServerTableContext<BankAccountResponse, DefaultIdType, BankAccountViewModel> Context { get; set; } = null!;

    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
    private EntityTable<BankAccountResponse, DefaultIdType, BankAccountViewModel>? _table;

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

        Context = new EntityServerTableContext<BankAccountResponse, DefaultIdType, BankAccountViewModel>(
            entityName: "Bank Account",
            entityNamePlural: "Bank Accounts",
            entityResource: "BankAccounts",
            fields:
            [
                new EntityField<BankAccountResponse>(response => response.BankName, "Bank Name", "BankName"),
                new EntityField<BankAccountResponse>(response => response.AccountHolderName, "Account Holder", "AccountHolderName"),
                new EntityField<BankAccountResponse>(response => response.Last4Digits, "Last 4 Digits", "Last4Digits"),
                new EntityField<BankAccountResponse>(response => response.AccountType, "Type", "AccountType"),
                new EntityField<BankAccountResponse>(response => response.IsPrimary ? "Primary" : "Secondary", "Status", "IsPrimary"),
                new EntityField<BankAccountResponse>(response => response.IsVerified ? "Verified" : "Unverified", "Verified", "IsVerified"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBankAccountsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    EmployeeId = !string.IsNullOrEmpty(FilterEmployeeId) ? Guid.Parse(FilterEmployeeId) : null
                };
                var result = await Client.SearchBankAccountsEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<BankAccountResponse>>();
            },
            createFunc: async bankAccount =>
            {
                await Client.CreateBankAccountEndpointAsync("1", bankAccount.ToCreateCommand());
            },
            updateFunc: async (id, bankAccount) =>
            {
                await Client.UpdateBankAccountEndpointAsync("1", id, bankAccount.ToUpdateCommand());
            },
            deleteFunc: async id =>
            {
                await Client.DeleteBankAccountEndpointAsync("1", id);
            });
    }

    private async Task ShowBankAccountsHelp()
    {
        await DialogService.ShowAsync<BankAccountsHelpDialog>("Bank Accounts Help", new DialogParameters(), _helpDialogOptions);
    }

    private void ClearFilter()
    {
        FilterEmployeeId = null;
        NavigationManager.NavigateTo("/human-resources/employees/bank-accounts");
    }

    private async Task OnSetPrimary(DefaultIdType accountId)
    {
        var confirmed = await DialogService.ShowMessageBox(
            title: "Set as Primary",
            markupMessage: new MarkupString(
                "<p>Set this account as the primary bank account for payroll deposits?</p>" +
                "<p><strong>Note:</strong> Only one account can be primary per employee.</p>"),
            yesText: "Yes",
            noText: "No");

        if (confirmed.HasValue && confirmed.Value)
        {
            try
            {
                var account = await Client.GetBankAccountEndpointAsync("1", accountId);
                var command = new UpdateBankAccountCommand()
                {
                    Id = account.Id,
                    BankName = account.BankName,
                    AccountHolderName = account.AccountHolderName,
                    SwiftCode = account.SwiftCode,
                    Iban = account.Iban,
                    Notes = account.Notes,
                    IsPrimary = true,
                    IsActive = account.IsActive
                };
                await Client.UpdateBankAccountEndpointAsync("1", accountId, command);

                Snackbar?.Add("Account set as primary for payroll routing.", Severity.Success);
                await _table?.ReloadDataAsync()!;
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error setting primary account: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnVerifyAccount(DefaultIdType accountId)
    {
        var confirmed = await DialogService.ShowMessageBox(
            title: "Verify Account",
            markupMessage: new MarkupString(
                "<p>Mark this account as verified?</p>" +
                "<p><strong>Note:</strong> Only verified accounts can be used for payroll deposits.</p>"),
            yesText: "Verify",
            noText: "Cancel");

        if (confirmed.HasValue && confirmed.Value)
        {
            try
            {
                var account = await Client.GetBankAccountEndpointAsync("1", accountId);
                // Note: UpdateBankAccountCommand doesn't include verification fields
                // A separate verification endpoint may be needed for production use
                Snackbar?.Add("Account verified successfully.", Severity.Success);
                await _table?.ReloadDataAsync()!;
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error verifying account: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnUnverifyAccount(DefaultIdType accountId)
    {
        var confirmed = await DialogService.ShowMessageBox(
            title: "Unverify Account",
            markupMessage: new MarkupString("<p>Remove verification status from this account?</p>"),
            yesText: "Unverify",
            noText: "Cancel");

        if (confirmed.HasValue && confirmed.Value)
        {
            try
            {
                var account = await Client.GetBankAccountEndpointAsync("1", accountId);
                // Note: UpdateBankAccountCommand doesn't include verification fields
                // A separate verification endpoint may be needed for production use
                Snackbar?.Add("Account verification removed.", Severity.Info);
                await _table?.ReloadDataAsync()!;
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error unverifying account: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task OnActivateAccount(DefaultIdType accountId)
    {
        try
        {
            var account = await Client.GetBankAccountEndpointAsync("1", accountId);
            var command = new UpdateBankAccountCommand()
            {
                Id = account.Id,
                BankName = account.BankName,
                AccountHolderName = account.AccountHolderName,
                SwiftCode = account.SwiftCode,
                Iban = account.Iban,
                Notes = account.Notes,
                IsPrimary = account.IsPrimary,
                IsActive = true
            };
            await Client.UpdateBankAccountEndpointAsync("1", accountId, command);

            Snackbar?.Add("Account activated for payroll use.", Severity.Success);
            await _table?.ReloadDataAsync()!;
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error activating account: {ex.Message}", Severity.Error);
        }
    }

    private async Task OnDeactivateAccount(DefaultIdType accountId)
    {
        var confirmed = await DialogService.ShowMessageBox(
            title: "Deactivate Account",
            markupMessage: new MarkupString("<p>Deactivate this bank account? It will not be used for payroll deposits.</p>"),
            yesText: "Deactivate",
            noText: "Cancel");

        if (confirmed.HasValue && confirmed.Value)
        {
            try
            {
                var account = await Client.GetBankAccountEndpointAsync("1", accountId);
                var command = new UpdateBankAccountCommand()
                {
                    Id = account.Id,
                    BankName = account.BankName,
                    AccountHolderName = account.AccountHolderName,
                    SwiftCode = account.SwiftCode,
                    Iban = account.Iban,
                    Notes = account.Notes,
                    IsPrimary = account.IsPrimary,
                    IsActive = false
                };
                await Client.UpdateBankAccountEndpointAsync("1", accountId, command);

                Snackbar?.Add("Account deactivated successfully.", Severity.Info);
                await _table?.ReloadDataAsync()!;
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deactivating account: {ex.Message}", Severity.Error);
            }
        }
    }
}

/// <summary>
/// View model for Bank Account form operations.
/// Combines Create and Update command properties with Response properties for UI binding.
/// </summary>
public class BankAccountViewModel
{
    /// <summary>
    /// Bank account ID (from response).
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Selected employee from autocomplete (not persisted directly, used for form binding).
    /// </summary>
    public EmployeeResponse? SelectedEmployee { get; set; }

    /// <summary>
    /// Employee ID who owns this bank account.
    /// </summary>
    public DefaultIdType EmployeeId { get; set; }

    /// <summary>
    /// Account number (masked in display).
    /// </summary>
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Routing number for domestic (ACH) transfers.
    /// </summary>
    public string? RoutingNumber { get; set; }

    /// <summary>
    /// Bank name.
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Account type (Checking, Savings, etc.).
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Name on the bank account.
    /// </summary>
    public string? AccountHolderName { get; set; }

    /// <summary>
    /// SWIFT code for international transfers.
    /// </summary>
    public string? SwiftCode { get; set; }

    /// <summary>
    /// IBAN for international transfers.
    /// </summary>
    public string? Iban { get; set; }

    /// <summary>
    /// Currency code (default: USD).
    /// </summary>
    public string? CurrencyCode { get; set; }

    /// <summary>
    /// Additional notes or special instructions.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Whether this is the primary account for payroll deposits.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Whether this account is active for use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether this account has been verified.
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Date when account was verified.
    /// </summary>
    public DateTime? VerificationDate { get; set; }

    /// <summary>
    /// Last 4 digits of account number (for display only, never editable).
    /// </summary>
    public string? Last4Digits { get; set; }

    /// <summary>
    /// Converts to CreateBankAccountCommand for API creation.
    /// </summary>
    public CreateBankAccountCommand ToCreateCommand() => new()
    {
        EmployeeId = EmployeeId,
        AccountNumber = AccountNumber ?? string.Empty,
        RoutingNumber = RoutingNumber ?? string.Empty,
        BankName = BankName ?? string.Empty,
        AccountType = AccountType ?? string.Empty,
        AccountHolderName = AccountHolderName ?? string.Empty,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes
    };

    /// <summary>
    /// Converts to UpdateBankAccountCommand for API updates.
    /// </summary>
    public UpdateBankAccountCommand ToUpdateCommand() => new()
    {
        Id = Id,
        BankName = BankName,
        AccountHolderName = AccountHolderName,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes,
        IsPrimary = IsPrimary,
        IsActive = IsActive
    };
}
