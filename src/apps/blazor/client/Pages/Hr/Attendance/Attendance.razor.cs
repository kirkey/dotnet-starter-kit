using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.Attendance;

public partial class Attendance
{
    

    protected EntityServerTableContext<AttendanceResponse, DefaultIdType, AttendanceViewModel> Context { get; set; } = null!;
    
    private ClientPreference _preference = new();

    private readonly DialogOptions _helpDialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Large, 
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

        Context = new EntityServerTableContext<AttendanceResponse, DefaultIdType, AttendanceViewModel>(
            entityName: "Attendance",
            entityNamePlural: "Attendance Records",
            entityResource: "Attendance",
            fields:
            [
                new EntityField<AttendanceResponse>(r => r.Id.ToString(), "ID", "Id"),
                new EntityField<AttendanceResponse>(r => r.Status ?? "Unknown", "Status", "Status"),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchAttendanceRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchAttendanceEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<AttendanceResponse>>();
            },
            createFunc: async _ =>
            {
                var command = new CreateAttendanceCommand();
                await Client.CreateAttendanceEndpointAsync("1", command);
            },
            updateFunc: async (id, _) =>
            {
                var command = new UpdateAttendanceCommand();
                await Client.UpdateAttendanceEndpointAsync("1", id, command);
            },
            deleteFunc: async id =>
            {
                await Client.DeleteAttendanceEndpointAsync("1", id);
            });
    }

    private async Task ShowAttendanceHelp()
    {
        await DialogService.ShowAsync<AttendanceHelpDialog>("Attendance Help", new DialogParameters(), _helpDialogOptions);
    }
}

/// <summary>
/// View model for Attendance form operations.
/// Combines Create and Update command properties with Response properties for UI binding.
/// </summary>
public class AttendanceViewModel
{
    /// <summary>
    /// Attendance record ID.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Selected employee from autocomplete.
    /// </summary>
    public EmployeeResponse? SelectedEmployee { get; set; }

    /// <summary>
    /// Employee ID.
    /// </summary>
    public DefaultIdType EmployeeId { get; set; }

    /// <summary>
    /// Attendance date.
    /// </summary>
    public DateTime? AttendanceDate { get; set; }

    /// <summary>
    /// Attendance status (Present, Absent, Late, Leave, Half-Day).
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Clock in time.
    /// </summary>
    public TimeSpan? InTime { get; set; }

    /// <summary>
    /// Clock out time.
    /// </summary>
    public TimeSpan? OutTime { get; set; }

    /// <summary>
    /// Selected shift from autocomplete.
    /// </summary>
    public ShiftResponse? SelectedShift { get; set; }

    /// <summary>
    /// Shift ID.
    /// </summary>
    public DefaultIdType? ShiftId { get; set; }

    /// <summary>
    /// Whether attendance is verified.
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Whether attendance is approved.
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Converts to CreateAttendanceCommand for API creation.
    /// </summary>
    public CreateAttendanceCommand ToCreateCommand() => new();

    /// <summary>
    /// Converts to UpdateAttendanceCommand for API updates.
    /// </summary>
    public UpdateAttendanceCommand ToUpdateCommand() => new();
}

