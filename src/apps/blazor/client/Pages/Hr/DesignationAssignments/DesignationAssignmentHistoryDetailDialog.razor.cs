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

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}

