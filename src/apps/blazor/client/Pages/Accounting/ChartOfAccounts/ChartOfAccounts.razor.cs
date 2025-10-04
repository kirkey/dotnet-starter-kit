namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

public partial class ChartOfAccounts
{
    protected EntityServerTableContext<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = default!;

    private EntityTable<ChartOfAccountResponse, DefaultIdType, ChartOfAccountViewModel> _table = default!;

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
                var paginationFilter = filter.Adapt<SearchChartOfAccountQuery>();

                var result = await Client.ChartOfAccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<ChartOfAccountResponse>>();
            },
            createFunc: async account =>
            {
                await Client.ChartOfAccountCreateEndpointAsync("1", account.Adapt<CreateChartOfAccountCommand>());
            },
            updateFunc: async (id, account) =>
            {
                await Client.ChartOfAccountUpdateEndpointAsync("1", id, account.Adapt<UpdateChartOfAccountCommand>());
            },
            deleteFunc: async id => await Client.ChartOfAccountDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class ChartOfAccountViewModel : UpdateChartOfAccountCommand
{
    // Properties inherited from UpdateChartOfAccountCommand
}
