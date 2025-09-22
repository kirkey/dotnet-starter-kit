namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class Budgets
{
    [Inject] protected IClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<BudgetResponse, DefaultIdType, BudgetViewModel> Context { get; set; } = default!;

    private EntityTable<BudgetResponse, DefaultIdType, BudgetViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<BudgetResponse, DefaultIdType, BudgetViewModel>(
            entityName: "Accounting Period",
            entityNamePlural: "Accounting Periods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<BudgetResponse>(response => response.PeriodName, "Period", "PeriodName"),
                new EntityField<BudgetResponse>(response => response.Name, "Name", "Name"),
                new EntityField<BudgetResponse>(response => response.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<BudgetResponse>(response => response.BudgetType, "Type", "BudgetType"),
                new EntityField<BudgetResponse>(response => response.TotalBudgetedAmount, "Total Budgeted", "TotalBudgetedAmount", typeof(decimal)),
                new EntityField<BudgetResponse>(response => response.TotalActualAmount, "Total Actual", "TotalActualAmount", typeof(decimal)),
                new EntityField<BudgetResponse>(response => response.Description, "Description", "Description"),
                new EntityField<BudgetResponse>(response => response.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchBudgetsQuery>();

                var result = await ApiClient.BudgetSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<BudgetResponse>>();
            },
            createFunc: async period =>
            {
                await ApiClient.BudgetCreateEndpointAsync("1", period.Adapt<CreateBudgetCommand>());
            },
            updateFunc: async (id, period) =>
            {
                await ApiClient.BudgetUpdateEndpointAsync("1", id, period.Adapt<UpdateBudgetCommand>());
            },
            deleteFunc: async id => await ApiClient.BudgetDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class BudgetViewModel
{
    // Core identifiers
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }

    // Fields from CreateBudgetRequest / BudgetDto
    public DefaultIdType PeriodId { get; set; }
    public string? PeriodName { get; set; }
    public int FiscalYear { get; set; }
    public string? BudgetType { get; set; }

    // Optional status used in UpdateBudgetRequest
    public string? Status { get; set; }

    // Budget totals from BudgetDto
    public decimal TotalBudgetedAmount { get; set; }
    public decimal TotalActualAmount { get; set; }

    // Approval info from BudgetDto
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }

    // Common descriptive fields
    public string? Description { get; set; }
    public string? Notes { get; set; }
}
