using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Components.Common;

/// <summary>
/// Global error notification component that displays error details at the bottom of the page.
/// Subscribes to the GlobalErrorService to show errors from anywhere in the application.
/// </summary>
public partial class ErrorNotification : IDisposable
{
    [Inject] private GlobalErrorService ErrorService { get; set; } = default!;
    [Inject] private NavigationManager NavigationService { get; set; } = default!;

    private bool _isVisible;
    private ErrorDetails? _errorDetails;

    protected override void OnInitialized()
    {
        ErrorService.OnError += HandleError;
        ErrorService.OnClear += ClearError;
    }

    /// <summary>
    /// Handles error events from the global error service.
    /// </summary>
    /// <param name="errorDetails">The error details to display.</param>
    private void HandleError(ErrorDetails errorDetails)
    {
        _errorDetails = errorDetails;
        _isVisible = true;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Clears the current error display.
    /// </summary>
    private void ClearError()
    {
        _isVisible = false;
        _errorDetails = null;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles the reload button click event.
    /// </summary>
    private void Reload()
    {
        _isVisible = false;
        _errorDetails = null;
        NavigationService.NavigateTo(NavigationService.Uri, forceLoad: true);
    }

    /// <summary>
    /// Handles the dismiss button click event.
    /// </summary>
    private void Dismiss()
    {
        _isVisible = false;
        _errorDetails = null;
        StateHasChanged();
    }

    /// <summary>
    /// Disposes the component and unsubscribes from error service events.
    /// </summary>
    public void Dispose()
    {
        ErrorService.OnError -= HandleError;
        ErrorService.OnClear -= ClearError;
        GC.SuppressFinalize(this);
    }
}

