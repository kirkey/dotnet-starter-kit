using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.Deductions;

public partial class Deductions
{
    protected EntityServerTableContext<DeductionResponse, DefaultIdType, DeductionViewModel> Context { get; set; } = null!;

    private EntityTable<DeductionResponse, DefaultIdType, DeductionViewModel>? _table;

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

        Context = new EntityServerTableContext<DeductionResponse, DefaultIdType, DeductionViewModel>(
            entityName: "Deduction",
            entityNamePlural: "Deductions",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<DeductionResponse>(r => r.DeductionName ?? "-", "Name", "DeductionName"),
                new EntityField<DeductionResponse>(r => r.DeductionType ?? "-", "Type", "DeductionType"),
                new EntityField<DeductionResponse>(r => r.RecoveryMethod ?? "-", "Recovery Method", "RecoveryMethod"),
                new EntityField<DeductionResponse>(r => r.RecoveryFixedAmount?.ToString("C2") ?? "-", "Fixed Amount", "RecoveryFixedAmount"),
                new EntityField<DeductionResponse>(r => r.RecoveryPercentage?.ToString("P2") ?? "-", "Percentage", "RecoveryPercentage"),
                new EntityField<DeductionResponse>(r => r.IsRecurring ? "Yes" : "No", "Recurring", "IsRecurring"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchDeductionsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchDeductionsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<DeductionResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateDeductionCommand
                {
                    DeductionName = vm.DeductionName,
                    DeductionType = vm.DeductionType,
                    RecoveryMethod = vm.RecoveryMethod,
                    RecoveryFixedAmount = vm.RecoveryFixedAmount,
                    RecoveryPercentage = vm.RecoveryPercentage,
                    InstallmentCount = vm.InstallmentCount,
                    MaxRecoveryPercentage = vm.MaxRecoveryPercentage,
                    RequiresApproval = vm.RequiresApproval,
                    IsRecurring = vm.IsRecurring,
                    GlAccountCode = vm.GlAccountCode,
                    Description = vm.Description
                };
                await Client.CreateDeductionEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateDeductionCommand
                {
                    Id = id,
                    DeductionName = vm.DeductionName,
                    DeductionType = vm.DeductionType,
                    RecoveryMethod = vm.RecoveryMethod,
                    RecoveryFixedAmount = vm.RecoveryFixedAmount,
                    RecoveryPercentage = vm.RecoveryPercentage,
                    InstallmentCount = vm.InstallmentCount,
                    MaxRecoveryPercentage = vm.MaxRecoveryPercentage,
                    RequiresApproval = vm.RequiresApproval,
                    IsRecurring = vm.IsRecurring,
                    GlAccountCode = vm.GlAccountCode,
                    Description = vm.Description
                };
                await Client.UpdateDeductionEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteDeductionEndpointAsync("1", id).ConfigureAwait(false));

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<DeductionsHelpDialog>("Deductions Help", new DialogParameters(), _dialogOptions);
    }
}

public class DeductionViewModel
{
    public DefaultIdType Id { get; set; }
    public string? DeductionName { get; set; }
    public string? DeductionType { get; set; } = "Loan";
    public string? RecoveryMethod { get; set; } = "Manual";
    public decimal? RecoveryFixedAmount { get; set; }
    public decimal? RecoveryPercentage { get; set; }
    public int? InstallmentCount { get; set; }
    public decimal MaxRecoveryPercentage { get; set; } = 20M;
    public bool RequiresApproval { get; set; } = true;
    public bool IsRecurring { get; set; }
    public string? GlAccountCode { get; set; }
    public string? Description { get; set; }
}
