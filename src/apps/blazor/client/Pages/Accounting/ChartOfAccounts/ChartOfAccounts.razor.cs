namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

public partial class ChartOfAccounts
{
    protected EntityServerTableContext<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = null!;

    private EntityTable<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> _table = null!;

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };

    protected override Task OnInitializedAsync()
    {
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
                var command = account.Adapt<CreateChartOfAccountCommand>();
                command.AccountCode = account.AccountCode!.ToUpperInvariant();
                command.Name = account.Name!.ToUpperInvariant();
                await Client.ChartOfAccountCreateEndpointAsync("1", command);
            },
            updateFunc: async (id, account) =>
            {
                var command = account.Adapt<UpdateChartOfAccountCommand>();
                command.AccountCode = account.AccountCode!.ToUpperInvariant();
                command.Name = account.Name!.ToUpperInvariant();
                await Client.ChartOfAccountUpdateEndpointAsync("1", id, command);
            },
            deleteFunc: async id => await Client.ChartOfAccountDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
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
