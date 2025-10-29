using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Components;

/// <summary>
/// Helper service for executing API calls with proper error handling and user notifications.
/// Handles API exceptions and provides consistent error messaging throughout the application.
/// </summary>
public class ApiHelper(ISnackbar snackbar, NavigationManager navigationManager, GlobalErrorService errorService)
{
    public async Task<T?> ExecuteCallGuardedAsync<T>(
        Func<Task<T>> call,
        FshValidation? customValidation = null,
        string? successMessage = null)
    {
        customValidation?.ClearErrors();
        try
        {
            var result = await call();

            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                snackbar.Add(successMessage, Severity.Info);
            }

            return result;
        }
        catch (ApiException ex)
        {
            if (ex.StatusCode == 401)
            {
                navigationManager.NavigateTo("/logout");
            }
            var message = ex.Message switch
            {
                "TypeError: Failed to fetch" => "Unable to Reach API",
                _ => ex.Message
            };
            snackbar.Add(message, Severity.Error);
            errorService.ShowError(ex);
        }

        return default;
    }

    public async Task<bool> ExecuteCallGuardedAsync(
        Func<Task> call,
        FshValidation? customValidation = null,
        string? successMessage = null)
    {
        customValidation?.ClearErrors();
        try
        {
            await call();

            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                snackbar.Add(successMessage, Severity.Success);
            }

            return true;
        }
        catch (ApiException ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            errorService.ShowError(ex);
        }

        return false;
    }
}
