namespace FSH.Starter.Blazor.Client.Pages.Hr.PayrollLines;

public partial class PayrollLines
{
    protected EntityServerTableContext<PayrollLineResponse, DefaultIdType, PayrollLineViewModel> Context { get; set; } = null!;

    private EntityTable<PayrollLineResponse, DefaultIdType, PayrollLineViewModel>? _table;

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

        Context = new EntityServerTableContext<PayrollLineResponse, DefaultIdType, PayrollLineViewModel>(
            entityName: "Payroll Line",
            entityNamePlural: "Payroll Lines",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<PayrollLineResponse>(r => r.RegularHours.ToString("F1"), "Reg Hours", "RegularHours"),
                new EntityField<PayrollLineResponse>(r => r.OvertimeHours.ToString("F1"), "OT Hours", "OvertimeHours"),
                new EntityField<PayrollLineResponse>(r => r.GrossPay.ToString("C2"), "Gross Pay", "GrossPay"),
                new EntityField<PayrollLineResponse>(r => r.TotalDeductions.ToString("C2"), "Deductions", "TotalDeductions"),
                new EntityField<PayrollLineResponse>(r => r.NetPay.ToString("C2"), "Net Pay", "NetPay"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPayrollLinesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchPayrollLinesEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PayrollLineResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePayrollLineCommand
                {
                    PayrollId = vm.PayrollId,
                    EmployeeId = vm.EmployeeId,
                    RegularHours = vm.RegularHours ?? 160,
                    OvertimeHours = vm.OvertimeHours ?? 0
                };
                await Client.CreatePayrollLineEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePayrollLineCommand
                {
                    Id = id,
                    RegularHours = vm.RegularHours,
                    OvertimeHours = vm.OvertimeHours,
                    RegularPay = vm.RegularPay,
                    OvertimePay = vm.OvertimePay,
                    BonusPay = vm.BonusPay
                };
                await Client.UpdatePayrollLineEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePayrollLineEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<PayrollLinesHelpDialog>("Payroll Lines Help", new DialogParameters(), _dialogOptions);
    }
}

public class PayrollLineViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType PayrollId { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public decimal? RegularHours { get; set; } = 160;
    public decimal? OvertimeHours { get; set; } = 0;
    public decimal? RegularPay { get; set; }
    public decimal? OvertimePay { get; set; }
    public decimal? BonusPay { get; set; }
    public decimal? OtherEarnings { get; set; }
}
