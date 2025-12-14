namespace FSH.Starter.Blazor.Client.Pages.Hr.PerformanceReviews;

/// <summary>
/// Performance Reviews page for managing employee performance evaluations.
/// Provides CRUD operations and workflow for Submit, Complete, and Acknowledge.
/// </summary>
public partial class PerformanceReviews
{
    protected EntityServerTableContext<PerformanceReviewResponse, DefaultIdType, PerformanceReviewViewModel> Context { get; set; } = null!;

    private EntityTable<PerformanceReviewResponse, DefaultIdType, PerformanceReviewViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _dialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Medium, 
        FullWidth = true 
    };

    /// <summary>
    /// Initializes the component and sets up the entity table context with CRUD operations.
    /// </summary>
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

        Context = new EntityServerTableContext<PerformanceReviewResponse, DefaultIdType, PerformanceReviewViewModel>(
            entityName: "Performance Review",
            entityNamePlural: "Performance Reviews",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<PerformanceReviewResponse>(r => r.EmployeeName ?? "-", "Employee", "EmployeeName"),
                new EntityField<PerformanceReviewResponse>(r => r.ReviewerName ?? "-", "Reviewer", "ReviewerName"),
                new EntityField<PerformanceReviewResponse>(r => r.ReviewType ?? "-", "Type", "ReviewType"),
                new EntityField<PerformanceReviewResponse>(r => r.ReviewPeriodStart.ToShortDateString(), "Period Start", "ReviewPeriodStart"),
                new EntityField<PerformanceReviewResponse>(r => r.ReviewPeriodEnd.ToShortDateString(), "Period End", "ReviewPeriodEnd"),
                new EntityField<PerformanceReviewResponse>(r => r.OverallRating.ToString("0.0"), "Rating", "OverallRating"),
                new EntityField<PerformanceReviewResponse>(r => r.Status ?? "Draft", "Status", "Status"),
            ],
            enableAdvancedSearch: false,
            idFunc: r => r.Id,
            searchFunc: async filter =>
            {
                var request = new SearchPerformanceReviewsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize
                };
                var result = await Client.SearchPerformanceReviewsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<PerformanceReviewResponse>>();
            },
            createFunc: async vm =>
            {
                var command = new CreatePerformanceReviewCommand
                {
                    EmployeeId = vm.SelectedEmployee?.Id ?? vm.EmployeeId!.Value,
                    ReviewerId = vm.SelectedReviewer?.Id ?? vm.ReviewerId!.Value,
                    ReviewPeriodStart = vm.ReviewPeriodStart!.Value,
                    ReviewPeriodEnd = vm.ReviewPeriodEnd!.Value,
                    ReviewType = vm.ReviewType ?? "Annual",
                    OverallRating = vm.OverallRating,
                    Strengths = vm.Strengths,
                    AreasForImprovement = vm.AreasForImprovement,
                    Goals = vm.Goals,
                    ReviewerComments = vm.ReviewerComments,
                    EmployeeComments = vm.EmployeeComments
                };
                await Client.CreatePerformanceReviewEndpointAsync("1", command).ConfigureAwait(false);
            },
            updateFunc: async (id, vm) =>
            {
                var command = new UpdatePerformanceReviewCommand
                {
                    Id = id,
                    OverallRating = vm.OverallRating,
                    Strengths = vm.Strengths,
                    AreasForImprovement = vm.AreasForImprovement,
                    Goals = vm.Goals,
                    ReviewerComments = vm.ReviewerComments,
                    EmployeeComments = vm.EmployeeComments
                };
                await Client.UpdatePerformanceReviewEndpointAsync("1", id, command).ConfigureAwait(false);
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the help dialog with information about performance reviews.
    /// </summary>
    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<PerformanceReviewHelpDialog>("Performance Review Help", new DialogParameters(), _dialogOptions);
    }

    /// <summary>
    /// Submits a performance review for completion.
    /// </summary>
    private async Task SubmitReviewAsync(PerformanceReviewResponse review)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Submit Performance Review",
            "Are you sure you want to submit this performance review? The review will be sent for completion.",
            yesText: "Submit", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.SubmitPerformanceReviewEndpointAsync("1", review.Id).ConfigureAwait(false);
                Snackbar.Add("Performance review submitted successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error submitting review: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Completes a submitted performance review.
    /// </summary>
    private async Task CompleteReviewAsync(PerformanceReviewResponse review)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Complete Performance Review",
            "Are you sure you want to mark this performance review as complete? The employee will be notified to acknowledge.",
            yesText: "Complete", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.CompletePerformanceReviewEndpointAsync("1", review.Id).ConfigureAwait(false);
                Snackbar.Add("Performance review completed successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error completing review: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Acknowledges a completed performance review by the employee.
    /// </summary>
    private async Task AcknowledgeReviewAsync(PerformanceReviewResponse review)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Acknowledge Performance Review",
            "By acknowledging, you confirm that you have read and understood the performance review.",
            yesText: "Acknowledge", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                await Client.AcknowledgePerformanceReviewEndpointAsync("1", review.Id).ConfigureAwait(false);
                Snackbar.Add("Performance review acknowledged successfully", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error acknowledging review: {ex.Message}", Severity.Error);
            }
        }
    }

    /// <summary>
    /// Shows detailed view of a performance review.
    /// </summary>
    private async Task ViewDetailsAsync(PerformanceReviewResponse review)
    {
        var parameters = new DialogParameters
        {
            { nameof(PerformanceReviewDetailsDialog.Review), review }
        };
        await DialogService.ShowAsync<PerformanceReviewDetailsDialog>("Performance Review Details", parameters, _dialogOptions);
    }
}
