namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class BudgetDetails
{
    [Parameter]
    public DefaultIdType BudgetId { get; set; }

    

    protected EntityClientTableContext<BudgetDetailResponse, DefaultIdType, BudgetDetailViewModel> Context { get; set; } = null!;

    private EntityTable<BudgetDetailResponse, DefaultIdType, BudgetDetailViewModel> _table = null!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityClientTableContext<BudgetDetailResponse, DefaultIdType, BudgetDetailViewModel>(
            entityName: "Budget Line",
            entityNamePlural: "Budget Lines",
            entityResource: FshResources.Accounting,
            searchAction: FshActions.View,
            fields:
            [
                new EntityField<BudgetDetailResponse>(response => response.AccountId, "Account", "AccountId"),
                new EntityField<BudgetDetailResponse>(response => response.BudgetedAmount, "Budgeted", "BudgetedAmount", typeof(decimal)),
                new EntityField<BudgetDetailResponse>(response => response.ActualAmount, "Actual", "ActualAmount", typeof(decimal)),
                new EntityField<BudgetDetailResponse>(response => response.Description, "Description", "Description"),
            ],
            idFunc: response => response.Id,
            loadDataFunc: async () =>
            {
                var result = await Client.BudgetDetailSearchEndpointAsync("1", BudgetId);
                return result.Adapt<List<BudgetDetailResponse>>();
            },
            searchFunc: (searchString, detail) =>
                string.IsNullOrWhiteSpace(searchString)
                || detail.Description?.Contains(searchString, StringComparison.OrdinalIgnoreCase) == true,
            createFunc: async period =>
            {
                var command = period.Adapt<CreateBudgetDetailCommand>();
                command.BudgetId = BudgetId;
                await Client.BudgetDetailCreateEndpointAsync("1", command);
            },
            updateFunc: async (id, period) =>
            {
                var command = period.Adapt<UpdateBudgetDetailCommand>();
                if (BudgetId != command.Id) return;
                await Client.BudgetDetailUpdateEndpointAsync("1", BudgetId, command);
            },
            deleteFunc: async id => await Client.BudgetDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}
