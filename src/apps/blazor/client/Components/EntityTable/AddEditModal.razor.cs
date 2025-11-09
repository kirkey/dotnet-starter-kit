namespace FSH.Starter.Blazor.Client.Components.EntityTable;

public partial class AddEditModal<TRequest> : IAddEditModal<TRequest>
{
    [Parameter]
    [EditorRequired]
    public RenderFragment<TRequest> ChildContent { get; set; } = null!;
    [Parameter]
    [EditorRequired]
    public TRequest RequestModel { get; set; } = default!;
    [Parameter]
    [EditorRequired]
    public Func<TRequest, Task> SaveFunc { get; set; } = null!;
    [Parameter]
    public Func<Task>? OnInitializedFunc { get; set; }
    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = null!;
    [Parameter]
    public bool IsCreate { get; set; }
    [Parameter]
    public bool IsViewMode { get; set; }
    [Parameter]
    public string? SuccessMessage { get; set; }

    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private FshValidation? _customValidation;

    public void ForceRender() => StateHasChanged();

    protected override Task OnInitializedAsync() =>
        OnInitializedFunc is not null
            ? OnInitializedFunc()
            : Task.CompletedTask;

    private async Task SaveAsync()
    {
        if (await ApiHelper.ExecuteCallGuardedAsync(
            () => SaveFunc(RequestModel), _customValidation, SuccessMessage))
        {
            MudDialog.Close();
        }
    }
}
