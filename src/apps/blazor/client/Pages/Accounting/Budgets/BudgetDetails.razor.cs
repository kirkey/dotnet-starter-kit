// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client/Pages/Accounting/Budgets/BudgetLines.razor.cs
namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class BudgetDetails
{
    [Parameter]
    public Guid BudgetId { get; set; }

    [Inject] protected IClient ApiClient { get; set; } = default!;

    protected EntityClientTableContext<BudgetLineResponse, DefaultIdType, BudgetDetailViewModel> Context { get; set; } = default!;

    private EntityTable<BudgetLineResponse, DefaultIdType, BudgetDetailViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityClientTableContext<BudgetLineResponse, DefaultIdType, BudgetDetailViewModel>(
            entityName: "Budget Line",
            entityNamePlural: "Budget Lines",
            entityResource: FshResources.Accounting,
            searchAction: FshActions.View,
            fields:
            [
                new EntityField<BudgetLineResponse>(dto => dto.AccountId, "Account", "AccountId"),
                new EntityField<BudgetLineResponse>(dto => dto.BudgetedAmount, "Budgeted", "BudgetedAmount", typeof(decimal)),
                new EntityField<BudgetLineResponse>(dto => dto.ActualAmount, "Actual", "ActualAmount", typeof(decimal)),
                new EntityField<BudgetLineResponse>(dto => dto.Description, "Description", "Description"),
            ],
            idFunc: response => response.Id,
            loadDataFunc: async () =>
            {
                var result = await ApiClient.BudgetGetLinesEndpointAsync("1", BudgetId);
                return result.Adapt<List<BudgetLineResponse>>();
            },
            searchFunc: (searchString, detail) =>
                string.IsNullOrWhiteSpace(searchString)
                || detail.Description?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true,
            createFunc: async period =>
            {
                await ApiClient.BudgetAddLineEndpointAsync("1", BudgetId, period.Adapt<AddBudgetLineCommand>());
            },
            updateFunc: async (id, period) =>
            {
                await ApiClient.BudgetUpdateLineEndpointAsync("1", BudgetId, id, period.Adapt<UpdateBudgetLineCommand>());
            },
            deleteFunc: async id => await ApiClient.BudgetDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}
