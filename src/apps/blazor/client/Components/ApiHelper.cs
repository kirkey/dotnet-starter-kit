namespace FSH.Starter.Blazor.Client.Components;

public static class ApiHelper
{
    private static readonly ISnackbar Snackbar = default!;
    private static readonly NavigationManager NavigationManager = default!;
    
    public static async Task<T?> ExecuteCallGuardedAsync<T>(
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
                Snackbar.Add(successMessage, Severity.Info);
            }

            return result;
        }
        catch (ApiException ex)
        {
            if (ex.StatusCode == 401)
            {
                NavigationManager.NavigateTo("/logout");
            }
            var message = ex.Message switch
            {
                "TypeError: Failed to fetch" => "Unable to Reach API",
                _ => ex.Message
            };
            Snackbar.Add(message, Severity.Error);
        }

        return default;
    }

    public static async Task<bool> ExecuteCallGuardedAsync(
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
                Snackbar.Add(successMessage, Severity.Success);
            }

            return true;
        }
        catch (ApiException ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }

        return false;
    }
}
