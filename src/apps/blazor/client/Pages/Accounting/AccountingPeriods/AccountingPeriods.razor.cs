using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.AccountingPeriods;

public partial class AccountingPeriods
{
    [Inject]
    protected IApiClient ApiClient { get; set; } = default!;

    protected EntityServerTableContext<AccountingPeriodDto, DefaultIdType, AccountingPeriodViewModel> Context { get; set; } = default!;

    private EntityTable<AccountingPeriodDto, DefaultIdType, AccountingPeriodViewModel> _table = default!;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<AccountingPeriodDto, DefaultIdType, AccountingPeriodViewModel>(
            entityName: "Accounting Period",
            entityNamePlural: "Accounting Periods",
            entityResource: FshResources.Accounting,
            fields:
            [
                new EntityField<AccountingPeriodDto>(dto => dto.Name, "Name", "Name"),
                new EntityField<AccountingPeriodDto>(dto => dto.StartDate, "Start Date", "StartDate", typeof(DateTime)),
                new EntityField<AccountingPeriodDto>(dto => dto.EndDate, "End Date", "EndDate", typeof(DateTime)),
                new EntityField<AccountingPeriodDto>(dto => dto.IsAdjustmentPeriod, "Adjustment Period", "IsAdjustmentPeriod", typeof(bool)),
                new EntityField<AccountingPeriodDto>(dto => dto.FiscalYear, "Fiscal Year", "FiscalYear", typeof(int)),
                new EntityField<AccountingPeriodDto>(dto => dto.PeriodType, "Period Type", "PeriodType"),
                new EntityField<AccountingPeriodDto>(dto => dto.Description, "Description", "Description"),
                new EntityField<AccountingPeriodDto>(dto => dto.Notes, "Notes", "Notes"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<SearchAccountingPeriodsRequest>();

                var result = await ApiClient.AccountingPeriodSearchEndpointAsync("1", paginationFilter);
                return result.Adapt<PaginationResponse<AccountingPeriodDto>>();
            },
            createFunc: async period =>
            {
                await ApiClient.AccountingPeriodCreateEndpointAsync("1", period.Adapt<CreateAccountingPeriodRequest>());
            },
            updateFunc: async (id, period) =>
            {
                await ApiClient.AccountingPeriodUpdateEndpointAsync("1", id, period.Adapt<UpdateAccountingPeriodRequest>());
            },
            deleteFunc: async id => await ApiClient.AccountingPeriodDeleteEndpointAsync("1", id));

        return Task.CompletedTask;
    }
}

public class AccountingPeriodViewModel : UpdateAccountingPeriodRequest
{
    // Properties will be inherited from UpdateAccountingPeriodRequest
    // This class serves as the view model for the entity table
}
