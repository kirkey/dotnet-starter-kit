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
            deleteFunc: async id => await Client.ChartOfAccountDeleteEndpointAsync("1", id));

        await Task.CompletedTask;
    }

    private async Task ShowChartOfAccountsHelp()
    {
        await DialogService.ShowAsync<ChartOfAccountsHelpDialog>("Chart of Accounts Help", new DialogParameters(), _helpDialogOptions);
    }
}

public class ChartOfAccountViewModel : UpdateChartOfAccountCommand
{
    // Properties inherited from UpdateChartOfAccountCommand
}
