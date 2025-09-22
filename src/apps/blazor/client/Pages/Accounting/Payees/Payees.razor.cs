namespace FSH.Starter.Blazor.Client.Pages.Accounting.Payees;

public partial class Payees : ComponentBase
{
    [Inject]
    protected IClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<PayeeResponse, DefaultIdType, ResponseViewModel> Context { get; set; } = default!;

    private EntityTable<PayeeResponse, DefaultIdType, ResponseViewModel> _table = default!;

    protected override void OnInitialized() =>
        Context = new EntityServerTableContext<PayeeResponse, DefaultIdType, ResponseViewModel>(
            entityName: "Chart Of Account",
            entityNamePlural: "Chart Of Accounts",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<PayeeResponse>(response => response.PayeeCode, "Payee Code", "PayeeCode"),
                new EntityField<PayeeResponse>(response => response.Name, "Name", "Name"),
                new EntityField<PayeeResponse>(response => response.ExpenseAccountCode, "Account Code", "ExpenseAccountCode"),
                new EntityField<PayeeResponse>(response => response.ExpenseAccountName, "Account Name", "ExpenseAccountName"),
                new EntityField<PayeeResponse>(response => response.Description, "Description", "Description"),
                new EntityField<PayeeResponse>(response => response.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PayeeSearchCommand>();

                var result = await ApiClient.PayeeSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<PayeeResponse>>();
            },
            createFunc: async viewModel =>
            {
                await ApiClient.PayeeCreateEndpointAsync("1", viewModel.Adapt<PayeeCreateCommand>());
            },
            updateFunc: async (id, viewModel) =>
            {
                await ApiClient.PayeeUpdateEndpointAsync("1", id, viewModel.Adapt<PayeeUpdateCommand>());
            },
            deleteFunc: async id => await ApiClient.PayeeDeleteEndpointAsync("1", id));
}

public class ResponseViewModel : PayeeUpdateCommand;
