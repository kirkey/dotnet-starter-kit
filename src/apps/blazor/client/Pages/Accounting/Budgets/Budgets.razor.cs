using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class Budgets
{
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<BudgetDto, DefaultIdType, BudgetViewModel> Context { get; set; } = default!;

    private EntityTable<BudgetDto, DefaultIdType, BudgetViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<BudgetDto, DefaultIdType, BudgetViewModel>(
            entityName: "Accounting Period",
            entityNamePlural: "Accounting Periods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BudgetDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<BudgetDto>(dto => dto.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<BudgetDto>(dto => dto.BudgetType, "Type", "BudgetType"),
                new EntityField<BudgetDto>(dto => dto.TotalBudgetedAmount, "Total Budgeted", "TotalBudgetedAmount", typeof(decimal)),
                new EntityField<BudgetDto>(dto => dto.TotalActualAmount, "Total Actual", "TotalActualAmount", typeof(decimal)),
                new EntityField<BudgetDto>(dto => dto.Description, "Description", "Description"),
                new EntityField<BudgetDto>(dto => dto.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchBudgetsRequest>();

                var result = await ApiClient.BudgetSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<BudgetDto>>();
            },
            createFunc: async period =>
            {
                await ApiClient.BudgetCreateEndpointAsync("1", period.Adapt<CreateBudgetRequest>());
            },
            updateFunc: async (id, period) =>
            {
                await ApiClient.BudgetUpdateEndpointAsync("1", id, period.Adapt<UpdateBudgetRequest>());
            },
            deleteFunc: async id => await ApiClient.BudgetDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class BudgetViewModel : UpdateBudgetRequest
{
    // Properties will be inherited from UpdateBudgetRequest
    // This class serves as the view model for the entity table
}
