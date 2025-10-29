# Global Error Notification System

## Overview
A global error notification component that displays API errors at the bottom of the page across all pages in the application without requiring setup on individual pages.

## Architecture

### Components Created

1. **GlobalErrorService** (`/Services/GlobalErrorService.cs`)
   - Singleton service registered in DI container
   - Manages global error state using events
   - Methods:
     - `ShowError(ErrorDetails)` - Display error with details
     - `ShowError(Exception)` - Display error from exception
     - `ShowError(string, string?)` - Display error with message
     - `ClearErrors()` - Clear all errors

2. **ErrorDetails** (in `GlobalErrorService.cs`)
   - Data class for error information
   - Properties:
     - `Message` - Error message
     - `Details` - Detailed error info
     - `StatusCode` - HTTP status code
     - `Timestamp` - When error occurred
   - Static method `FromApiException()` to extract error details from API exceptions

3. **ErrorNotification Component** (`/Components/Common/ErrorNotification.razor`)
   - Global UI component displayed at bottom of page
   - Fixed position with slide-up animation
   - Features:
     - Error message display
     - HTTP status code badge
     - Expandable technical details
     - Reload page button
     - Dismiss button
   - Auto-subscribes to GlobalErrorService events

## Integration

### 1. Service Registration
The `GlobalErrorService` is registered in `Program.cs`:
```csharp
builder.Services.AddSingleton<GlobalErrorService>();
```

### 2. Component Placement
The `ErrorNotification` component is added to `App.razor` to make it available globally:
```razor
<ErrorNotification />
```

### 3. ApiHelper Integration
The `ApiHelper` class automatically uses `GlobalErrorService` to display API errors:
```csharp
catch (ApiException ex)
{
    snackbar.Add(message, Severity.Error);
    errorService.ShowError(ex);  // Shows global error notification
}
```

## Usage

### Automatic Error Handling
Most API errors are automatically caught and displayed by the `ApiHelper`:
- Search operations
- Create operations
- Update operations
- Delete operations

### Manual Error Display
You can manually show errors from anywhere in the application:

```csharp
// Inject the service
[Inject] private GlobalErrorService ErrorService { get; set; } = default!;

// Show error with exception
try
{
    // Your code
}
catch (Exception ex)
{
    ErrorService.ShowError(ex);
}

// Show error with message
ErrorService.ShowError("Operation failed", "Detailed error information");

// Clear errors
ErrorService.ClearErrors();
```

## User Experience

### Error Display
- Appears at bottom center of screen
- Slides up with animation
- Fixed position (doesn't scroll with content)
- High z-index (9999) to appear above all content

### Error Actions
- **Reload Page** - Refreshes the current page
- **Dismiss (X)** - Closes the error notification
- **View Technical Details** - Expandable section showing detailed error information

### Visual Design
- Red accent border on left
- Error icon and "Error Occurred" title
- Status code badge (if available)
- Dark mode support
- Professional, non-blocking UI

## Benefits

1. **Global Setup** - No need to add error handling to each page
2. **Consistent UX** - Same error display across all pages
3. **Professional** - Better than default error boundaries
4. **Informative** - Shows detailed error information from API
5. **User-Friendly** - Clear actions (reload or dismiss)
6. **Developer-Friendly** - Easy to use and extend

## Examples

The error notification automatically works on all pages including:
- Inventory Transactions
- Purchase Orders
- Items
- Warehouses
- All CRUD operations via EntityTable

No additional code needed on individual pages!

