using FSH.Starter.Blazor.Infrastructure.Api;
namespace FSH.Starter.Blazor.Client.Pages.Hr.DesignationAssignments;

/// <summary>
/// Code-behind for DesignationAssignmentHistoryDetailDialog.
/// Displays detailed assignment history for an employee.
/// </summary>
public partial class DesignationAssignmentHistoryDetailDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public EmployeeHistoryDto? EmployeeHistory { get; set; }

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }
    }

    
}

