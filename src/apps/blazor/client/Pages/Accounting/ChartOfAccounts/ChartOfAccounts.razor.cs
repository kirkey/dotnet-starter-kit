namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

public partial class ChartOfAccounts
{
    protected EntityServerTableContext<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = null!;

    private EntityTable<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> _table = null!;

    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

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

        Context = new EntityServerTableContext<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel>(
            entityName: "Chart Of Account",
            entityNamePlural: "Chart Of Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ChartOfAccountResponse>(response => response.UsoaCategory, "Category", "UsoaCategory"),
                new EntityField<ChartOfAccountResponse>(response => response.AccountType, "Type", "AccountType"),
                new EntityField<ChartOfAccountResponse>(response => response.ParentCode, "Parent", "ParentCode"),
                new EntityField<ChartOfAccountResponse>(response => response.AccountCode, "Code", "AccountCode"),
                new EntityField<ChartOfAccountResponse>(response => response.Name, "Name", "Name"),
                new EntityField<ChartOfAccountResponse>(response => response.Balance, "Balance", "Balance", typeof(decimal)),
                new EntityField<ChartOfAccountResponse>(response => response.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchChartOfAccountRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.ChartOfAccountSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<ChartOfAccountResponse>>();
            },
            createFunc: async account =>
            {
                account.AccountCode = account.AccountCode?.ToUpperInvariant() ?? account.AccountCode;
                account.Name = account.Name?.ToUpperInvariant() ?? account.Name;
                await Client.ChartOfAccountCreateEndpointAsync("1", account.Adapt<CreateChartOfAccountCommand>());
            },
            updateFunc: async (id, account) =>
            {
                account.AccountCode = account.AccountCode?.ToUpperInvariant() ?? account.AccountCode;
                account.Name = account.Name?.ToUpperInvariant() ?? account.Name;
                await Client.ChartOfAccountUpdateEndpointAsync("1", id, account.Adapt<UpdateChartOfAccountCommand>());
            },
            deleteFunc: async id => await Client.ChartOfAccountDeleteEndpointAsync("1", id),
            hasExtraActionsFunc: () => true);

        await Task.CompletedTask;
    }

    private async Task ShowChartOfAccountsHelp()
    {
        await DialogService.ShowAsync<ChartOfAccountsHelpDialog>("Chart of Accounts Help", new DialogParameters(), _helpDialogOptions);
    }

    private async Task ViewDetailsAsync(ChartOfAccountResponse account)
    {
        var parameters = new DialogParameters
        {
            { nameof(ChartOfAccountDetailsDialog.Account), account }
        };
        await DialogService.ShowAsync<ChartOfAccountDetailsDialog>("Chart of Account Details", parameters, _helpDialogOptions);
    }

    private async Task ActivateAccountAsync(ChartOfAccountResponse account)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Activate Account",
            $"Are you sure you want to activate account '{account.AccountCode} - {account.Name}'?",
            yesText: "Activate", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.ChartOfAccountActivateEndpointAsync("1", account.Id).ConfigureAwait(false);
                Snackbar.Add("Account activated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error activating account: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task DeactivateAccountAsync(ChartOfAccountResponse account)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Deactivate Account",
            $"Are you sure you want to deactivate account '{account.AccountCode} - {account.Name}'? This may affect related transactions.",
            yesText: "Deactivate", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.ChartOfAccountDeactivateEndpointAsync("1", account.Id).ConfigureAwait(false);
                Snackbar.Add("Account deactivated successfully", Severity.Success);
                await _table.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error deactivating account: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task ShowImportDialog()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        var dialog = await DialogService.ShowAsync<ChartOfAccountImportDialog>("Import Chart of Accounts", parameters, options);
        var result = await dialog.Result;
        
        if (result is { Canceled: false })
        {
            await _table.ReloadDataAsync();
        }
    }

    private async Task ShowExportDialog()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small, FullWidth = true };
        await DialogService.ShowAsync<ChartOfAccountExportDialog>("Export Chart of Accounts", parameters, options);
    }
}

public class ChartOfAccountViewModel : UpdateChartOfAccountCommand
{
    // Properties inherited from UpdateChartOfAccountCommand
}
