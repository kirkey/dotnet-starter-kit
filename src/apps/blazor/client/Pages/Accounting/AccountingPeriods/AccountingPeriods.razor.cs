using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountingPeriods;

public partial class AccountingPeriods
{
    [Inject] protected IApiClient ApiClient { get; set; } = default!;

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
                new EntityField<AccountingPeriodResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<AccountingPeriodResponse>(dto => dto.StartDate, "Start Date", "StartDate", typeof(DateTime)),
                new EntityField<AccountingPeriodResponse>(dto => dto.EndDate, "End Date", "EndDate", typeof(DateTime)),
                new EntityField<AccountingPeriodResponse>(dto => dto.IsAdjustmentPeriod, "Adjustment Period", "IsAdjustmentPeriod", typeof(bool)),
                new EntityField<AccountingPeriodResponse>(dto => dto.FiscalYear, "Fiscal Year", "FiscalYear", typeof(int)),
                new EntityField<AccountingPeriodResponse>(dto => dto.PeriodType, "Period Type", "PeriodType"),
                new EntityField<AccountingPeriodResponse>(dto => dto.Description, "Description", "Description"),
                new EntityField<AccountingPeriodResponse>(dto => dto.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchAccountingPeriodsQuery>();

                var result = await ApiClient.AccountingPeriodSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<AccountingPeriodResponse>>();
            },
            createFunc: async period =>
            {
                await ApiClient.AccountingPeriodCreateEndpointAsync("1", period.Adapt<CreateAccountingPeriodCommand>());
            },
            updateFunc: async (id, period) =>
            {
                await ApiClient.AccountingPeriodUpdateEndpointAsync("1", id, period.Adapt<UpdateAccountingPeriodCommand>());
            },
            deleteFunc: async id => await ApiClient.AccountingPeriodDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class AccountingPeriodViewModel : UpdateAccountingPeriodCommand
{
    // Properties will be inherited from UpdateAccountingPeriodRequest
    // This class serves as the view model for the entity table
}
