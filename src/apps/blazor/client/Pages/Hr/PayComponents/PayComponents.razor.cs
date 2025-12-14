namespace FSH.Starter.Blazor.Client.Pages.Hr.PayComponents;

public partial class PayComponents
{
    protected EntityServerTableContext<PayComponentResponse, DefaultIdType, PayComponentViewModel> Context { get; set; } = null!;

    private EntityTable<PayComponentResponse, DefaultIdType, PayComponentViewModel>? _table;

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

        Context = new EntityServerTableContext<PayComponentResponse, DefaultIdType, PayComponentViewModel>(
            entityName: "Pay Component",
            entityNamePlural: "Pay Components",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<PayComponentResponse>(r => r.Code ?? "-", "Code", "Code"),
                new EntityField<PayComponentResponse>(r => r.ComponentName ?? "-", "Name", "ComponentName"),
                new EntityField<PayComponentResponse>(r => r.ComponentType ?? "-", "Type", "ComponentType"),
                new EntityField<PayComponentResponse>(r => r.CalculationMethod ?? "-", "Method", "CalculationMethod"),
                new EntityField<PayComponentResponse>(r => r.FixedAmount?.ToString("C2") ?? "-", "Fixed Amt", "FixedAmount"),
                new EntityField<PayComponentResponse>(r => r.IsSubjectToTax ? "Yes" : "No", "Taxable", "IsSubjectToTax"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPayComponentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchPayComponentsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PayComponentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePayComponentCommand
                {
                    Code = vm.Code,
                    ComponentName = vm.ComponentName,
                    ComponentType = vm.ComponentType,
                    CalculationMethod = vm.CalculationMethod,
                    GlAccountCode = vm.GlAccountCode,
                    Description = vm.Description,
                    CalculationFormula = vm.CalculationFormula,
                    Rate = vm.Rate,
                    FixedAmount = vm.FixedAmount,
                    MinValue = vm.MinValue,
                    MaxValue = vm.MaxValue,
                    IsCalculated = vm.IsCalculated,
                    IsMandatory = vm.IsMandatory,
                    IsSubjectToTax = vm.IsSubjectToTax,
                    IsTaxExempt = vm.IsTaxExempt,
                    LaborLawReference = vm.LaborLawReference
                };
                await Client.CreatePayComponentEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePayComponentCommand
                {
                    Id = id,
                    ComponentName = vm.ComponentName,
                    CalculationMethod = vm.CalculationMethod,
                    CalculationFormula = vm.CalculationFormula,
                    Rate = vm.Rate,
                    FixedAmount = vm.FixedAmount,
                    GlAccountCode = vm.GlAccountCode,
                    Description = vm.Description
                };
                await Client.UpdatePayComponentEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePayComponentEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<PayComponentsHelpDialog>("Pay Components Help", new DialogParameters(), _dialogOptions);
    }
}

public class PayComponentViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Code { get; set; }
    public string? ComponentName { get; set; }
    public string? ComponentType { get; set; } = "Earnings";
    public string? CalculationMethod { get; set; } = "Manual";
    public string? GlAccountCode { get; set; }
    public string? Description { get; set; }
    public string? CalculationFormula { get; set; }
    public decimal? Rate { get; set; }
    public decimal? FixedAmount { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public bool IsCalculated { get; set; } = true;
    public bool IsMandatory { get; set; }
    public bool IsSubjectToTax { get; set; } = true;
    public bool IsTaxExempt { get; set; }
    public string? LaborLawReference { get; set; }
}
