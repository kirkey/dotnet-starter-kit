using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.BenefitAllocations;

/// <summary>
/// Benefit Allocations page for managing and tracking benefit allocation records.
/// </summary>
public partial class BenefitAllocations
{
    protected EntityServerTableContext<BenefitAllocationResponse, DefaultIdType, BenefitAllocationViewModel> Context { get; set; } = null!;

    private EntityTable<BenefitAllocationResponse, DefaultIdType, BenefitAllocationViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _dialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true 
    };

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<BenefitAllocationResponse, DefaultIdType, BenefitAllocationViewModel>(
            entityName: "Benefit Allocation",
            entityNamePlural: "Benefit Allocations",
            entityResource: FshResources.Benefits,
            fields:
            [
                new EntityField<BenefitAllocationResponse>(r => r.EmployeeName ?? "-", "Employee", "EmployeeName"),
                new EntityField<BenefitAllocationResponse>(r => r.BenefitName ?? "-", "Benefit", "BenefitName"),
                new EntityField<BenefitAllocationResponse>(r => r.AllocationDate.ToShortDateString(), "Allocation Date", "AllocationDate"),
                new EntityField<BenefitAllocationResponse>(r => r.AllocatedAmount.ToString("C2"), "Amount", "AllocatedAmount"),
                new EntityField<BenefitAllocationResponse>(r => r.AllocationType ?? "-", "Type", "AllocationType"),
                new EntityField<BenefitAllocationResponse>(r => r.Status ?? "-", "Status", "Status"),
                new EntityField<BenefitAllocationResponse>(r => r.ReferenceNumber ?? "-", "Reference #", "ReferenceNumber"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBenefitAllocationsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchBenefitAllocationsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BenefitAllocationResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateBenefitAllocationCommand
                {
                    EnrollmentId = vm.SelectedEnrollment?.Id ?? vm.EnrollmentId,
                    AllocationDate = vm.AllocationDate,
                    AllocatedAmount = vm.AllocatedAmount,
                    AllocationType = vm.AllocationType ?? "Usage",
                    ReferenceNumber = vm.ReferenceNumber,
                    Remarks = vm.Remarks
                };
                await Client.CreateBenefitAllocationEndpointAsync("1", command).ConfigureAwait(false);
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<BenefitAllocationsHelpDialog>("Benefit Allocations Help", new DialogParameters(), _dialogOptions);
    }

    private async Task ViewDetailsAsync(BenefitAllocationResponse allocation)
    {
        var parameters = new DialogParameters
        {
            { nameof(BenefitAllocationDetailsDialog.Allocation), allocation }
        };
        await DialogService.ShowAsync<BenefitAllocationDetailsDialog>("Benefit Allocation Details", parameters, _dialogOptions);
    }

    private async Task ApproveAllocationAsync(BenefitAllocationResponse allocation)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Approve Allocation",
            $"Are you sure you want to approve this benefit allocation for {allocation.EmployeeName}?",
            yesText: "Approve", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.ApproveBenefitAllocationEndpointAsync("1", allocation.Id).ConfigureAwait(false);
                Snackbar.Add("Benefit allocation approved successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error approving allocation: {ex.Message}", Severity.Error);
            }
        }
    }

    private async Task RejectAllocationAsync(BenefitAllocationResponse allocation)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Reject Allocation",
            $"Are you sure you want to reject this benefit allocation for {allocation.EmployeeName}?",
            yesText: "Reject", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.RejectBenefitAllocationEndpointAsync("1", allocation.Id).ConfigureAwait(false);
                Snackbar.Add("Benefit allocation rejected successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error rejecting allocation: {ex.Message}", Severity.Error);
            }
        }
    }
}

/// <summary>
/// View model for creating and updating benefit allocations.
/// </summary>
public class BenefitAllocationViewModel
{
    public DefaultIdType Id { get; set; }
    
    public DefaultIdType EnrollmentId { get; set; }
    public BenefitEnrollmentResponse? SelectedEnrollment { get; set; }
    
    public DateTime? AllocationDate { get; set; } = DateTime.Today;
    public decimal AllocatedAmount { get; set; }
    public string? AllocationType { get; set; } = "Usage";
    public string? ReferenceNumber { get; set; }
    public string? Remarks { get; set; }
}
