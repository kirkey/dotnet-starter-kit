using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.PayComponentRates;

public partial class PayComponentRates
{
    protected EntityServerTableContext<PayComponentRateResponse, DefaultIdType, PayComponentRateViewModel> Context { get; set; } = null!;

    private EntityTable<PayComponentRateResponse, DefaultIdType, PayComponentRateViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _dialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true 
    };

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<PayComponentRateResponse, DefaultIdType, PayComponentRateViewModel>(
            entityName: "Pay Component Rate",
            entityNamePlural: "Pay Component Rates",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<PayComponentRateResponse>(r => r.MinAmount.ToString("C0"), "Min Amount", "MinAmount"),
                new EntityField<PayComponentRateResponse>(r => r.MaxAmount.ToString("C0"), "Max Amount", "MaxAmount"),
                new EntityField<PayComponentRateResponse>(r => $"{(r.EmployeeRate ?? 0) * 100:F2}%", "Employee Rate", "EmployeeRate"),
                new EntityField<PayComponentRateResponse>(r => $"{(r.EmployerRate ?? 0) * 100:F2}%", "Employer Rate", "EmployerRate"),
                new EntityField<PayComponentRateResponse>(r => r.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPayComponentRatesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchPayComponentRatesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PayComponentRateResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePayComponentRateCommand
                {
                    PayComponentId = vm.PayComponentId,
                    MinAmount = vm.MinAmount ?? 0,
                    MaxAmount = vm.MaxAmount ?? 0,
                    Year = vm.Year ?? DateTime.Today.Year,
                    EmployeeRate = vm.EmployeeRate,
                    EmployerRate = vm.EmployerRate,
                    AdditionalEmployerRate = vm.AdditionalEmployerRate,
                    EmployeeAmount = vm.EmployeeAmount,
                    EmployerAmount = vm.EmployerAmount
                };
                await Client.CreatePayComponentRateEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePayComponentRateCommand
                {
                    Id = id,
                    EmployeeRate = vm.EmployeeRate,
                    EmployerRate = vm.EmployerRate,
                    AdditionalEmployerRate = vm.AdditionalEmployerRate,
                    EmployeeAmount = vm.EmployeeAmount,
                    EmployerAmount = vm.EmployerAmount
                };
                await Client.UpdatePayComponentRateEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePayComponentRateEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<PayComponentRatesHelpDialog>("Pay Component Rates Help", new DialogParameters(), _dialogOptions);
    }
}

public class PayComponentRateViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType PayComponentId { get; set; }
    public decimal? MinAmount { get; set; } = 4000;
    public decimal? MaxAmount { get; set; } = 4250;
    public int? Year { get; set; } = DateTime.Today.Year;
    public decimal? EmployeeRate { get; set; } = 0.045M;
    public decimal? EmployerRate { get; set; } = 0.095M;
    public decimal? AdditionalEmployerRate { get; set; } = 0.01M;
    public decimal? EmployeeAmount { get; set; }
    public decimal? EmployerAmount { get; set; }
}
