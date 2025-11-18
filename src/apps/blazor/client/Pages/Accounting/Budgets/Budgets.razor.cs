namespace FSH.Starter.Blazor.Client.Pages.Accounting.Budgets;

public partial class Budgets
{
    

    protected EntityServerTableContext<BudgetResponse, DefaultIdType, BudgetViewModel> Context { get; set; } = null!;

    private EntityTable<BudgetResponse, DefaultIdType, BudgetViewModel> _table = null!;

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
                var request = new SearchBudgetsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.BudgetSearchEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<BudgetResponse>>();
            },
            createFunc: async period =>
            {
                await Client.BudgetCreateEndpointAsync("1", period.Adapt<CreateBudgetCommand>());
            },
            updateFunc: async (id, period) =>
            {
                await Client.BudgetUpdateEndpointAsync("1", id, period.Adapt<UpdateBudgetCommand>());
            },
            deleteFunc: async id => await Client.BudgetDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Show budgets help dialog.
    /// </summary>
    private async Task ShowBudgetsHelp()
    {
        await DialogService.ShowAsync<BudgetsHelpDialog>("Budgets Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

public class BudgetViewModel
{
    // Core identifiers
    public DefaultIdType Id { get; set; }
    public string? Name { get; set; }

    // Fields from CreateBudgetRequest / BudgetDto
    public DefaultIdType? PeriodId { get; set; }
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
