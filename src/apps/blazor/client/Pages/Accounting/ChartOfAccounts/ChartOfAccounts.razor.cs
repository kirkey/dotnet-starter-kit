using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.ChartOfAccounts;

public partial class ChartOfAccounts
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> Context { get; set; } = default!;

    private EntityTable<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ChartOfAccountDto, DefaultIdType, ChartOfAccountViewModel>(
            entityName: "Chart Of Account",
            entityNamePlural: "Chart Of Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<ChartOfAccountDto>(dto => dto.UsoaCategory, "Category", "UsoaCategory"),
                new EntityField<ChartOfAccountDto>(dto => dto.AccountType, "Type", "AccountType"),
                new EntityField<ChartOfAccountDto>(dto => dto.ParentCode, "Parent", "ParentCode"),
                new EntityField<ChartOfAccountDto>(dto => dto.AccountCode, "Code", "AccountCode"),
                new EntityField<ChartOfAccountDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<ChartOfAccountDto>(dto => dto.Balance, "Balance", "Balance", typeof(decimal)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchChartOfAccountRequest>();

                var result = await ApiClient.ChartOfAccountSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<ChartOfAccountDto>>();
            },
            createFunc: async account =>
            {
                await ApiClient.ChartOfAccountCreateEndpointAsync("1", account.Adapt<CreateChartOfAccountRequest>());
            },
            updateFunc: async (id, account) =>
            {
                await ApiClient.ChartOfAccountUpdateEndpointAsync("1", id, account.Adapt<UpdateChartOfAccountRequest>());
            },
            deleteFunc: async id => await ApiClient.ChartOfAccountDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class ChartOfAccountViewModel : UpdateChartOfAccountRequest
{
    // Properties will be inherited from UpdateChartOfAccountRequest
    // This class serves as the view model for the entity table
}
