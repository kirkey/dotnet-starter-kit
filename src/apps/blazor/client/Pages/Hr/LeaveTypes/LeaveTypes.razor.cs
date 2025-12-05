using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveTypes;

public partial class LeaveTypes
{
    protected EntityServerTableContext<LeaveTypeResponse, DefaultIdType, LeaveTypeViewModel> Context { get; set; } = null!;
    private EntityTable<LeaveTypeResponse, DefaultIdType, LeaveTypeViewModel> _table = null!;

    private ClientPreference _preference = new();

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

        Context = new EntityServerTableContext<LeaveTypeResponse, DefaultIdType, LeaveTypeViewModel>(
            entityName: "Leave Type",
            entityNamePlural: "Leave Types",
            entityResource: "LeaveTypes",
            fields:
            [
                new EntityField<LeaveTypeResponse>(response => response.LeaveName, "Name", "LeaveName"),
                new EntityField<LeaveTypeResponse>(response => response.LeaveCode, "Code", "LeaveCode"),
                new EntityField<LeaveTypeResponse>(response => response.AnnualAllowance, "Allowance", "AnnualAllowance"),
                new EntityField<LeaveTypeResponse>(response => response.AccrualFrequency, "Frequency", "AccrualFrequency"),
                new EntityField<LeaveTypeResponse>(response => response.IsPaid, "Paid", "IsPaid", Type: typeof(bool)),
                new EntityField<LeaveTypeResponse>(response => response.IsActive, "Active", "IsActive", Type: typeof(bool))
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            searchFunc: async filter =>
            {
                var request = new SearchLeaveTypesRequest
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchLeaveTypesEndpointAsync("1", request);
                return result.Adapt<PaginationResponse<LeaveTypeResponse>>();
            },
            createFunc: async leaveType =>
            {
                await Client.CreateLeaveTypeEndpointAsync("1", leaveType.Adapt<CreateLeaveTypeCommand>());
            },
            updateFunc: async (id, leaveType) =>
            {
                await Client.UpdateLeaveTypeEndpointAsync("1", id, leaveType.Adapt<UpdateLeaveTypeCommand>());
            },
            deleteFunc: async id =>
            {
                await Client.DeleteLeaveTypeEndpointAsync("1", id);
            });
    }

    private async Task ShowHelp()
    {
        await DialogService.ShowAsync<LeaveTypesHelpDialog>("Leave Types Help",
            new DialogParameters(), new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}

public class LeaveTypeViewModel : UpdateLeaveTypeCommand
{
    // Properties inherited from UpdateLeaveTypeCommand
}
