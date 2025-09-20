namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

public partial class ChartOfAccounts
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

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
                new EntityField<ChartOfAccountResponse>(dto => dto.UsoaCategory, "Category", "UsoaCategory"),
                new EntityField<ChartOfAccountResponse>(dto => dto.AccountType, "Type", "AccountType"),
                new EntityField<ChartOfAccountResponse>(dto => dto.ParentCode, "Parent", "ParentCode"),
                new EntityField<ChartOfAccountResponse>(dto => dto.AccountCode, "Code", "AccountCode"),
                new EntityField<ChartOfAccountResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<ChartOfAccountResponse>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchChartOfAccountQuery>();

                var result = await ApiClient.ChartOfAccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<ChartOfAccountResponse>>();
            },
            createFunc: async account =>
            {
                await ApiClient.ChartOfAccountCreateEndpointAsync("1", account.Adapt<CreateChartOfAccountCommand>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.ChartOfAccountUpdateEndpointAsync("1", id, account.Adapt<UpdateChartOfAccountCommand>());
            },
            deleteFunc: async id => await ApiClient.ChartOfAccountDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class ChartOfAccountViewModel : UpdateChartOfAccountCommand
{
    // Properties inherited from UpdateChartOfAccountCommand
}
