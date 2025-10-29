namespace FSH.Starter.Blazor.Client.Services;

/// <summary>
/// Global error service for managing application-wide error notifications.
/// Provides centralized error handling and display across all pages.
/// </summary>
public class GlobalErrorService
{
    /// <summary>
    /// Event raised when an error occurs that should be displayed to the user.
    /// </summary>
    public event Action<ErrorDetails>? OnError;

    /// <summary>
    /// Event raised when errors should be cleared.
    /// </summary>
    public event Action? OnClear;

    /// <summary>
    /// Shows an error notification with the specified details.
    /// </summary>
    /// <param name="errorDetails">The error details to display.</param>
    public void ShowError(ErrorDetails errorDetails)
    {
        OnError?.Invoke(errorDetails);
    }

    /// <summary>
    /// Shows an error notification from an exception.
    /// </summary>
    /// <param name="exception">The exception to display.</param>
    public void ShowError(Exception exception)
    {
        var errorDetails = ErrorDetails.FromApiException(exception);
        OnError?.Invoke(errorDetails);
    }

    /// <summary>
    /// Shows an error notification with a simple message.
    /// </summary>
    /// <param name="message">The error message to display.</param>
    /// <param name="details">Optional detailed error information.</param>
    public void ShowError(string message, string? details = null)
    {
        var errorDetails = new ErrorDetails
        {
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
        OnError?.Invoke(errorDetails);
    }

    /// <summary>
    /// Clears all error notifications.
    /// </summary>
    public void ClearErrors()
    {
        OnClear?.Invoke();
    }
}

/// <summary>
/// Represents error details for display in the error notification component.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed error information from the API.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the error.
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates an ErrorDetails instance from an ApiException.
    /// </summary>
    /// <param name="exception">The API exception.</param>
    /// <returns>An ErrorDetails instance.</returns>
    public static ErrorDetails FromApiException(Exception exception)
    {
        var errorDetails = new ErrorDetails
        {
            Message = exception.Message,
            Timestamp = DateTime.UtcNow
        };

        // Check if it's an ApiException with status code
        var statusCodeProperty = exception.GetType().GetProperty("StatusCode");
        if (statusCodeProperty != null)
        {
            errorDetails.StatusCode = statusCodeProperty.GetValue(exception) as int?;
        }

        // Check if there's a response property with additional details
        var responseProperty = exception.GetType().GetProperty("Response");
        if (responseProperty != null)
        {
            var response = responseProperty.GetValue(exception);
            if (response != null)
            {
                errorDetails.Details = response.ToString();
            }
        }

        // If no specific details, use the inner exception or stack trace
        if (string.IsNullOrWhiteSpace(errorDetails.Details))
        {
            if (exception.InnerException != null)
            {
                errorDetails.Details = $"Inner Exception: {exception.InnerException.Message}";
            }
            else if (!string.IsNullOrWhiteSpace(exception.StackTrace))
            {
                errorDetails.Details = $"Exception Type: {exception.GetType().Name}\n\n{exception.StackTrace}";
            }
        }

        return errorDetails;
    }
}

