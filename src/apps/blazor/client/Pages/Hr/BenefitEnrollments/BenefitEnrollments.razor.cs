using FSH.Starter.Blazor.Infrastructure.Api;

namespace FSH.Starter.Blazor.Client.Pages.Hr.BenefitEnrollments;

/// <summary>
/// Benefit Enrollments page for managing employee benefit plan enrollments.
/// </summary>
public partial class BenefitEnrollments
{
    protected EntityServerTableContext<BenefitEnrollmentResponse, DefaultIdType, BenefitEnrollmentViewModel> Context { get; set; } = null!;

    private EntityTable<BenefitEnrollmentResponse, DefaultIdType, BenefitEnrollmentViewModel>? _table;

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

        Context = new EntityServerTableContext<BenefitEnrollmentResponse, DefaultIdType, BenefitEnrollmentViewModel>(
            entityName: "Benefit Enrollment",
            entityNamePlural: "Benefit Enrollments",
            entityResource: FshResources.Benefits,
            fields:
            [
                new EntityField<BenefitEnrollmentResponse>(r => r.EmployeeName ?? "-", "Employee", "EmployeeName"),
                new EntityField<BenefitEnrollmentResponse>(r => r.BenefitName ?? "-", "Benefit Plan", "BenefitName"),
                new EntityField<BenefitEnrollmentResponse>(r => r.CoverageLevel ?? "-", "Coverage", "CoverageLevel"),
                new EntityField<BenefitEnrollmentResponse>(r => r.EffectiveDate.ToShortDateString(), "Effective Date", "EffectiveDate"),
                new EntityField<BenefitEnrollmentResponse>(r => r.EmployeeContributionAmount.ToString("C2"), "Employee Contrib.", "EmployeeContributionAmount"),
                new EntityField<BenefitEnrollmentResponse>(r => r.IsActive ? "Active" : "Terminated", "Status", "IsActive"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchBenefitEnrollmentsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchBenefitEnrollmentsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<BenefitEnrollmentResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreateBenefitEnrollmentCommand
                {
                    EmployeeId = vm.SelectedEmployee?.Id ?? vm.EmployeeId,
                    BenefitId = vm.SelectedBenefit?.Id ?? vm.BenefitId,
                    EnrollmentDate = vm.EnrollmentDate,
                    EffectiveDate = vm.EffectiveDate,
                    CoverageLevel = vm.CoverageLevel ?? "Individual",
                    EmployeeContributionAmount = vm.EmployeeContributionAmount,
                    EmployerContributionAmount = vm.EmployerContributionAmount
                };
                await Client.CreateBenefitEnrollmentEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdateBenefitEnrollmentCommand
                {
                    Id = id,
                    CoverageLevel = vm.CoverageLevel,
                    EmployeeContributionAmount = vm.EmployeeContributionAmount,
                    EmployerContributionAmount = vm.EmployerContributionAmount
                };
                await Client.UpdateBenefitEnrollmentEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<BenefitEnrollmentsHelpDialog>("Benefit Enrollments Help", new DialogParameters(), _dialogOptions);
    }

    private async Task ViewDetailsAsync(BenefitEnrollmentResponse enrollment)
    {
        var parameters = new DialogParameters
        {
            { nameof(BenefitEnrollmentDetailsDialog.Enrollment), enrollment }
        };
        await DialogService.ShowAsync<BenefitEnrollmentDetailsDialog>("Benefit Enrollment Details", parameters, _dialogOptions);
    }

    private async Task TerminateEnrollmentAsync(BenefitEnrollmentResponse enrollment)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Terminate Enrollment",
            $"Are you sure you want to terminate the benefit enrollment for {enrollment.EmployeeName} ({enrollment.BenefitName})?",
            yesText: "Terminate", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.TerminateBenefitEnrollmentEndpointAsync("1", enrollment.Id).ConfigureAwait(false);
                Snackbar.Add("Benefit enrollment terminated successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error terminating enrollment: {ex.Message}", Severity.Error);
            }
        }
    }
}

/// <summary>
/// View model for creating and updating benefit enrollments.
/// </summary>
public class BenefitEnrollmentViewModel
{
    public DefaultIdType Id { get; set; }
    
    public DefaultIdType EmployeeId { get; set; }
    public EmployeeResponse? SelectedEmployee { get; set; }
    
    public DefaultIdType BenefitId { get; set; }
    public BenefitDto? SelectedBenefit { get; set; }
    
    public DateTime? EnrollmentDate { get; set; } = DateTime.Today;
    public DateTime? EffectiveDate { get; set; } = DateTime.Today.AddDays(1);
    
    public string? CoverageLevel { get; set; } = "Individual";
    
    public decimal EmployeeContributionAmount { get; set; }
    public decimal EmployerContributionAmount { get; set; }
}
