namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveRequests;

/// <summary>
/// Leave Requests page for managing employee leave requests.
/// Provides CRUD operations and approval workflow for leave management.
/// </summary>
public partial class LeaveRequests
{
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<LeaveRequestResponse, DefaultIdType, LeaveRequestViewModel> Context { get; set; } = null!;

    private EntityTable<LeaveRequestResponse, DefaultIdType, LeaveRequestViewModel>? _table;

    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

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

        Context = new EntityServerTableContext<LeaveRequestResponse, DefaultIdType, LeaveRequestViewModel>(
            entityName: "Leave Request",
            entityNamePlural: "Leave Requests",
            entityResource: FshResources.Leaves,
            fields:
            [
                new EntityField<LeaveRequestResponse>(response => response.StartDate.ToShortDateString(), "Start Date", "StartDate"),
                new EntityField<LeaveRequestResponse>(response => response.EndDate.ToShortDateString(), "End Date", "EndDate"),
                new EntityField<LeaveRequestResponse>(response => response.NumberOfDays, "Days", "NumberOfDays"),
                new EntityField<LeaveRequestResponse>(response => response.Reason ?? "-", "Reason", "Reason"),
                new EntityField<LeaveRequestResponse>(response => response.Status ?? "Draft", "Status", "Status"),
                new EntityField<LeaveRequestResponse>(response => response.IsActive, "Active", "IsActive", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchLeaveRequestsRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLeaveRequestsEndpointAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<LeaveRequestResponse>>();
            },
            createFunc: async leaveRequest =>
            {
                await Client.CreateLeaveRequestEndpointAsync("1", leaveRequest.Adapt<CreateLeaveRequestCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, leaveRequest) =>
            {
                await Client.UpdateLeaveRequestEndpointAsync("1", id, leaveRequest.Adapt<UpdateLeaveRequestCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteLeaveRequestEndpointAsync("1", id).ConfigureAwait(false);
            },
            hasExtraActionsFunc: () => true);

        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Shows the leave requests help dialog.
    /// </summary>
    private async Task ShowLeaveRequestsHelp()
    {
        await DialogService.ShowAsync<LeaveRequestsHelpDialog>("Leave Requests Help", new DialogParameters(), _helpDialogOptions);
    }

    /// <summary>
    /// Submits a leave request for approval.
    /// </summary>
    private async Task SubmitLeaveRequestAsync(LeaveRequestResponse request)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Submit Leave Request",
            "Are you sure you want to submit this leave request for approval?",
            yesText: "Submit", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                var command = new SubmitLeaveRequestCommand { Id = request.Id };
                await Client.SubmitLeaveRequestEndpointAsync("1", request.Id, command);
                Snackbar.Add("Leave request submitted", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Approves a submitted leave request.
    /// </summary>
    private async Task ApproveLeaveRequestAsync(LeaveRequestResponse request)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Approve Leave Request",
            "Are you sure you want to approve this leave request?",
            yesText: "Approve", cancelText: "Cancel");

        if (confirm == true)
        {
            try
            {
                var command = new ApproveLeaveRequestCommand { Id = request.Id };
                await Client.ApproveLeaveRequestEndpointAsync("1", request.Id, command);
                Snackbar.Add("Leave request approved", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Rejects a submitted leave request.
    /// </summary>
    private async Task RejectLeaveRequestAsync(LeaveRequestResponse request)
    {
        var parameters = new DialogParameters
        {
            { "ContentText", "Please provide a reason for rejecting this leave request:" },
            { "ButtonText", "Reject" },
            { "Color", Color.Error }
        };

        var dialog = await DialogService.ShowAsync<RejectLeaveDialog>("Reject Leave Request", parameters);
        var result = await dialog.Result;

        if (!result!.Canceled && result.Data is string reason)
        {
            try
            {
                var command = new RejectLeaveRequestCommand { Id = request.Id, Reason = reason };
                await Client.RejectLeaveRequestEndpointAsync("1", request.Id, command);
                Snackbar.Add("Leave request rejected", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }

    /// <summary>
    /// Cancels a leave request.
    /// </summary>
    private async Task CancelLeaveRequestAsync(LeaveRequestResponse request)
    {
        bool? confirm = await DialogService.ShowMessageBox(
            "Cancel Leave Request",
            "Are you sure you want to cancel this leave request?",
            yesText: "Cancel Request", cancelText: "Keep");

        if (confirm == true)
        {
            try
            {
                var command = new CancelLeaveRequestCommand { Id = request.Id };
                await Client.CancelLeaveRequestEndpointAsync("1", request.Id, command);
                Snackbar.Add("Leave request cancelled", Severity.Success);
                await _table!.ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }
    }
}
