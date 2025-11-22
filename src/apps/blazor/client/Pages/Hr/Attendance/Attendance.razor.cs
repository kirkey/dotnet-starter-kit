namespace FSH.Starter.Blazor.Client.Pages.Hr.Attendance;

public partial class Attendance
{
    protected EntityServerTableContext<AttendanceResponse, DefaultIdType, AttendanceViewModel> Context { get; set; } = null!;

    private readonly DialogOptions _helpDialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Large, 
        FullWidth = true 
    };

    protected override Task OnInitializedAsync()
    {
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

        return Task.CompletedTask;
    }

    private async Task ShowAttendanceHelp()
    {
        await DialogService.ShowAsync<AttendanceHelpDialog>("Attendance Help", new DialogParameters(), _helpDialogOptions);
    }
}

/// <summary>
/// View model for Attendance form operations.
/// </summary>
public class AttendanceViewModel
{
    public DefaultIdType Id { get; set; }
    public string? Status { get; set; }

    public CreateAttendanceCommand ToCreateCommand() => new();

    public UpdateAttendanceCommand ToUpdateCommand() => new();
}

