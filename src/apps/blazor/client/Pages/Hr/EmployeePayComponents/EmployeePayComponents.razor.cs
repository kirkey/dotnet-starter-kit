namespace FSH.Starter.Blazor.Client.Pages.Hr.EmployeePayComponents;

public partial class EmployeePayComponents
{
    protected EntityServerTableContext<EmployeePayComponentResponse, DefaultIdType, EmployeePayComponentViewModel> Context { get; set; } = null!;

    private EntityTable<EmployeePayComponentResponse, DefaultIdType, EmployeePayComponentViewModel>? _table;

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

        Context = new EntityServerTableContext<EmployeePayComponentResponse, DefaultIdType, EmployeePayComponentViewModel>(
            entityName: "Employee Pay Component",
            entityNamePlural: "Employee Pay Components",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<EmployeePayComponentResponse>(r => r.AssignmentType ?? "-", "Type", "AssignmentType"),
                new EntityField<EmployeePayComponentResponse>(r => r.FixedAmount?.ToString("C2") ?? "-", "Fixed Amount", "FixedAmount"),
                new EntityField<EmployeePayComponentResponse>(r => r.CustomRate?.ToString("C2") ?? "-", "Custom Rate", "CustomRate"),
                new EntityField<EmployeePayComponentResponse>(r => r.EffectiveStartDate.ToShortDateString(), "Start Date", "EffectiveStartDate"),
                new EntityField<EmployeePayComponentResponse>(r => r.IsActive ? "Active" : "Inactive", "Status", "IsActive"),
                new EntityField<EmployeePayComponentResponse>(r => r.IsOneTime ? "One-Time" : "Recurring", "Frequency", "IsOneTime"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchEmployeePayComponentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchEmployeePayComponentsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<EmployeePayComponentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateEmployeePayComponentCommand
                {
                    EmployeeId = vm.EmployeeId,
                    PayComponentId = vm.PayComponentId,
                    AssignmentType = vm.AssignmentType,
                    CustomRate = vm.CustomRate,
                    FixedAmount = vm.FixedAmount,
                    CustomFormula = vm.CustomFormula,
                    EffectiveStartDate = vm.EffectiveStartDate,
                    EffectiveEndDate = vm.EffectiveEndDate,
                    IsOneTime = vm.IsOneTime,
                    OneTimeDate = vm.OneTimeDate
                };
                await Client.CreateEmployeePayComponentEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateEmployeePayComponentCommand
                {
                    Id = id,
                    CustomRate = vm.CustomRate,
                    FixedAmount = vm.FixedAmount,
                    CustomFormula = vm.CustomFormula,
                    Remarks = vm.Remarks
                };
                await Client.UpdateEmployeePayComponentEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteEmployeePayComponentEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<EmployeePayComponentsHelpDialog>("Employee Pay Components Help", new DialogParameters(), _dialogOptions);
    }
}

public class EmployeePayComponentViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public DefaultIdType PayComponentId { get; set; }
    public string? AssignmentType { get; set; } = "Addition";
    public decimal? CustomRate { get; set; }
    public decimal? FixedAmount { get; set; }
    public string? CustomFormula { get; set; }
    public DateTime? EffectiveStartDate { get; set; } = DateTime.Today;
    public DateTime? EffectiveEndDate { get; set; }
    public bool IsOneTime { get; set; }
    public DateTime? OneTimeDate { get; set; }
    public string? Remarks { get; set; }
}
