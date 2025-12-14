namespace FSH.Starter.Blazor.Client.Pages.Hr.PayrollDeductions;

public partial class PayrollDeductions
{
    protected EntityServerTableContext<PayrollDeductionResponse, DefaultIdType, PayrollDeductionViewModel> Context { get; set; } = null!;

    private EntityTable<PayrollDeductionResponse, DefaultIdType, PayrollDeductionViewModel>? _table;

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

        Context = new EntityServerTableContext<PayrollDeductionResponse, DefaultIdType, PayrollDeductionViewModel>(
            entityName: "Payroll Deduction",
            entityNamePlural: "Payroll Deductions",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<PayrollDeductionResponse>(r => r.DeductionType ?? "-", "Type", "DeductionType"),
                new EntityField<PayrollDeductionResponse>(r => r.DeductionAmount.ToString("C2"), "Amount", "DeductionAmount"),
                new EntityField<PayrollDeductionResponse>(r => $"{r.DeductionPercentage:F1}%", "Percentage", "DeductionPercentage"),
                new EntityField<PayrollDeductionResponse>(r => r.StartDate.ToShortDateString(), "Start", "StartDate"),
                new EntityField<PayrollDeductionResponse>(r => r.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
                new EntityField<PayrollDeductionResponse>(r => r.IsAuthorized ? "Yes" : "No", "Authorized", "IsAuthorized"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPayrollDeductionsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchPayrollDeductionsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PayrollDeductionResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePayrollDeductionCommand
                {
                    PayComponentId = vm.PayComponentId,
                    DeductionType = vm.DeductionType,
                    DeductionAmount = vm.DeductionAmount ?? 0,
                    DeductionPercentage = vm.DeductionPercentage ?? 0,
                    IsAuthorized = vm.IsAuthorized,
                    IsRecoverable = vm.IsRecoverable
                };
                await Client.CreatePayrollDeductionEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePayrollDeductionCommand
                {
                    Id = id,
                    DeductionAmount = vm.DeductionAmount,
                    DeductionPercentage = vm.DeductionPercentage,
                    IsAuthorized = vm.IsAuthorized,
                    IsRecoverable = vm.IsRecoverable,
                    EndDate = vm.EndDate,
                    MaxDeductionLimit = vm.MaxDeductionLimit,
                    Remarks = vm.Remarks
                };
                await Client.UpdatePayrollDeductionEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeletePayrollDeductionEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<PayrollDeductionsHelpDialog>("Payroll Deductions Help", new DialogParameters(), _dialogOptions);
    }
}

public class PayrollDeductionViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType PayComponentId { get; set; }
    public string? DeductionType { get; set; } = "FixedAmount";
    public decimal? DeductionAmount { get; set; }
    public decimal? DeductionPercentage { get; set; }
    public bool IsAuthorized { get; set; } = true;
    public bool IsRecoverable { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MaxDeductionLimit { get; set; }
    public string? Remarks { get; set; }
}
