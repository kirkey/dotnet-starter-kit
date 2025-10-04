namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountingPeriods;

public partial class AccountingPeriods
{
    

    protected EntityServerTableContext<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel> Context { get; set; } = default!;

    private EntityTable<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<AccountingPeriodResponse, DefaultIdType, AccountingPeriodViewModel>(
            entityName: "Accounting Period",
            entityNamePlural: "Accounting Periods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountingPeriodResponse>(response => response.Name, "Name", "Name"),
                new EntityField<AccountingPeriodResponse>(response => response.StartDate, "Start Date", "StartDate", typeof(DateOnly)),
                new EntityField<AccountingPeriodResponse>(response => response.EndDate, "End Date", "EndDate", typeof(DateOnly)),
                new EntityField<AccountingPeriodResponse>(response => response.IsAdjustmentPeriod, "Adjustment Period", "IsAdjustmentPeriod", typeof(bool)),
                new EntityField<AccountingPeriodResponse>(response => response.FiscalYear, "Fiscal Year", "FiscalYear"),
                new EntityField<AccountingPeriodResponse>(response => response.PeriodType, "Period Type", "PeriodType"),
                new EntityField<AccountingPeriodResponse>(response => response.Description, "Description", "Description"),
                new EntityField<AccountingPeriodResponse>(response => response.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchAccountingPeriodsQuery>();

                var result = await Client.AccountingPeriodSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<AccountingPeriodResponse>>();
            },
            createFunc: async period =>
            {
                await Client.AccountingPeriodCreateEndpointAsync("1", period.Adapt<CreateAccountingPeriodCommand>());
            },
            updateFunc: async (id, period) =>
            {
                await Client.AccountingPeriodUpdateEndpointAsync("1", id, period.Adapt<UpdateAccountingPeriodCommand>());
            },
            deleteFunc: async id => await Client.AccountingPeriodDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class AccountingPeriodViewModel : UpdateAccountingPeriodCommand
{
    // Properties will be inherited from UpdateAccountingPeriodRequest
    // This class serves as the view model for the entity table
}
