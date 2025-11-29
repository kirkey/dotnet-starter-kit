namespace FSH.Starter.Blazor.Client.Pages.Hr.LeaveTypes;

public partial class LeaveTypes
{
    protected EntityServerTableContext<LeaveTypeResponse, DefaultIdType, LeaveTypeViewModel> Context { get; set; } = null!;

    protected override Task OnInitializedAsync()
    {
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

        return Task.CompletedTask;
    }
}

public class LeaveTypeViewModel : UpdateLeaveTypeCommand
{
    // Properties inherited from UpdateLeaveTypeCommand
}
