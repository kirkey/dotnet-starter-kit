namespace FSH.Starter.Blazor.Client.Pages.Hr.DesignationAssignments;

public partial class DesignationAssignments
{
    protected EntityServerTableContext<DesignationAssignmentResponse, DefaultIdType, DesignationAssignmentViewModel> Context { get; set; } = null!;

    private readonly DialogOptions _helpDialogOptions = new() { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };

    private List<EmployeeHistoryDto> _historyRecords = new();

    private bool _historyLoading;

    private string? _filterEmployeeId;

    private DateTime? _filterFromDate;

    private DateTime? _filterToDate;

    protected override Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<DesignationAssignmentResponse, DefaultIdType, DesignationAssignmentViewModel>(
            entityName: "Designation Assignment",
            entityNamePlural: "Designation Assignments",
            entityResource: FshResources.Employees,
            fields:
            [
                new EntityField<DesignationAssignmentResponse>(response => response.EmployeeNumber, "Employee #", "EmployeeNumber"),
                new EntityField<DesignationAssignmentResponse>(response => response.EmployeeName, "Employee", "EmployeeName"),
                new EntityField<DesignationAssignmentResponse>(response => response.DesignationTitle, "Designation", "DesignationTitle"),
                new EntityField<DesignationAssignmentResponse>(response => response.EffectiveDate, "Effective Date", "EffectiveDate", typeof(DateTime)),
                new EntityField<DesignationAssignmentResponse>(response => response.TenureDisplay, "Tenure", "TenureDisplay"),
                new EntityField<DesignationAssignmentResponse>(response => response.IsCurrentlyActive, "Active", "IsCurrentlyActive", typeof(bool)),
            ],
            enableAdvancedSearch: false,
            idFunc: response => response.Id,
            searchFunc: _ =>
            {
                // Return empty results as designation assignments are managed via individual operations
                return Task.FromResult(new PaginationResponse<DesignationAssignmentResponse>
                {
                    Items = new List<DesignationAssignmentResponse>(),
                    TotalCount = 0
                });
            },
            createFunc: async assignment =>
            {
                if (assignment.IsPlantilla)
                {
                    await Client.AssignPlantillaDesignationEndpointAsync("1", assignment.Adapt<AssignPlantillaDesignationCommand>());
                }
                else if (assignment.IsActingAs)
                {
                    await Client.AssignActingAsDesignationEndpointAsync("1", assignment.Adapt<AssignActingAsDesignationCommand>());
                }
            },
            updateFunc: async (id, assignment) =>
            {
                if (assignment.EndDate.HasValue)
                {
                    await Client.EndDesignationAssignmentEndpointAsync("1", id, new EndDesignationRequest { EndDate = assignment.EndDate.Value });
                }
            },
            deleteFunc: async _ =>
            {
                Snackbar?.Add("Use the 'End Assignment' action to end an assignment", Severity.Info);
                await Task.CompletedTask;
            });

        return Task.CompletedTask;
    }

    private async Task ShowDesignationAssignmentsHelp()
    {
        await DialogService.ShowAsync<DesignationAssignmentsHelpDialog>("Designation Assignments Help", new DialogParameters(), _helpDialogOptions);
    }

    private async Task LoadHistoryAsync()
    {
        try
        {
            _historyLoading = true;

            var request = new SearchEmployeeHistoryRequest
            {
                PageNumber = 1,
                PageSize = 50,
                FromDate = _filterFromDate,
                ToDate = _filterToDate,
                IncludeActingDesignations = true
            };

            var result = await Client.SearchEmployeeHistoryEndpointAsync("1", request).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(_filterEmployeeId) && DefaultIdType.TryParse(_filterEmployeeId, out var employeeId))
            {
                _historyRecords = result.Items?.Where(h => h.EmployeeId == employeeId).ToList() ?? new();
            }
            else
            {
                _historyRecords = result.Items?.ToList() ?? new();
            }

            if (_historyRecords.Count == 0)
            {
                Snackbar?.Add("No assignment history found", Severity.Info);
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading history: {ex.Message}", Severity.Error);
        }
        finally
        {
            _historyLoading = false;
        }
    }

    private async Task ShowHistoryDetail(EmployeeHistoryDto history)
    {
        var parameters = new DialogParameters { { "EmployeeHistory", history } };
        await DialogService.ShowAsync<DesignationAssignmentHistoryDetailDialog>(
            $"Designation History: {history.FullName}",
            parameters,
            new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true });
    }
}

