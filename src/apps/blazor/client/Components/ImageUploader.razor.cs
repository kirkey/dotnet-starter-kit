using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FSH.Starter.Blazor.Client.Components;

/// <summary>
/// Reusable image uploader component with drag-and-drop functionality.
/// Supports image preview, validation, and upload to server via FileUploadCommand.
/// </summary>
public partial class ImageUploader : ComponentBase
{
    #region Parameters

    /// <summary>
    /// Gets or sets the maximum file size allowed for upload.
    /// Default is 5MB.
    /// </summary>
    [Parameter] public long MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5MB

    /// <summary>
    /// Gets or sets the current image URL to display.
    /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
    [Parameter] public string? CurrentImageUrl { get; set; }
#pragma warning restore CA1056

    /// <summary>
    /// Gets or sets the uploaded image data.
    /// </summary>
    [Parameter] public FileUploadCommand? UploadedImage { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the uploaded image changes.
    /// </summary>
    [Parameter] public EventCallback<FileUploadCommand?> UploadedImageChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the current image URL changes.
    /// </summary>
    [Parameter] public EventCallback<string?> CurrentImageUrlChanged { get; set; }

    /// <summary>
    /// Gets or sets the upload label text.
    /// Default is "Upload Image".
    /// </summary>
    [Parameter] public string UploadLabel { get; set; } = "Upload Image";

    /// <summary>
    /// Gets or sets the alt text for the image preview.
    /// </summary>
    [Parameter] public string? AltText { get; set; }

    /// <summary>
    /// Gets or sets the base URI for constructing image URLs.
    /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
    [Parameter] public string? BaseUri { get; set; }
#pragma warning restore CA1056

    #endregion

    #region Private Fields

    /// <summary>
    /// Reference to the MudFileUpload component.
    /// </summary>
    private MudFileUpload<IBrowserFile>? _fileUpload;

    /// <summary>
    /// CSS class for drag and drop styling.
    /// </summary>
    private string _dragClass = string.Empty;

    /// <summary>
    /// The name of the currently selected file.
    /// </summary>
    private string _selectedFileName = string.Empty;

    #endregion

    #region Injected Services

    /// <summary>
    /// Injected snackbar service for displaying notifications.
    /// </summary>
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// Injected navigation manager for URL construction.
    /// </summary>
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the image preview URL to display.
    /// Returns the uploaded image data URL if available, otherwise the current image URL.
    /// </summary>
    /// <returns>The image URL to display in the preview.</returns>
#pragma warning disable CA1055 // URI-like return values should not be strings
    public string? GetImagePreviewUrl()
#pragma warning restore CA1055
    {
        if (!string.IsNullOrWhiteSpace(UploadedImage?.Data))
        {
            return $"data:{UploadedImage.Extension};base64,{UploadedImage.Data}";
        }

        if (!string.IsNullOrWhiteSpace(CurrentImageUrl))
        {
            var baseUri = BaseUri ?? NavigationManager.BaseUri.TrimEnd('/');
            return CurrentImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) 
                ? CurrentImageUrl 
                : $"{baseUri}/{CurrentImageUrl.TrimStart('/')}";
        }

        return null;
    }

    /// <summary>
    /// Converts a file size in bytes to a human-readable string.
    /// </summary>
    /// <param name="bytes">The file size in bytes.</param>
    /// <returns>A formatted string representing the file size.</returns>
    public static string GetFileSizeText(long bytes)
    {
        const long kb = 1024;
        const long mb = kb * 1024;
        const long gb = mb * 1024;

        return bytes switch
        {
            >= gb => $"{bytes / (double)gb:F2} GB",
            >= mb => $"{bytes / (double)mb:F2} MB",
            >= kb => $"{bytes / (double)kb:F2} KB",
            _ => $"{bytes} B"
        };
    }

    /// <summary>
    /// Opens the file picker dialog.
    /// </summary>
    public async Task OpenFilePicker()
    {
        if (_fileUpload != null)
        {
            await _fileUpload.OpenFilePickerAsync();
        }
    }

    /// <summary>
    /// Removes the current image by clearing both the uploaded image and current image URL.
    /// </summary>
    public async Task RemoveImage()
    {
        UploadedImage = null;
        _selectedFileName = string.Empty;
        
        await UploadedImageChanged.InvokeAsync(null);
        
        // Also clear current image URL if needed
        if (!string.IsNullOrWhiteSpace(CurrentImageUrl))
        {
            await CurrentImageUrlChanged.InvokeAsync(null);
        }
        
        if (_fileUpload != null)
        {
            await _fileUpload.ClearAsync();
        }
        
        StateHasChanged();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Handles the file selection change event.
    /// Validates the selected file and converts it to base64 format.
    /// </summary>
    /// <param name="e">The input file change event arguments.</param>
    private async Task OnFilesChanged(InputFileChangeEventArgs e)
    {
        try
        {
            if (e.FileCount == 0)
            {
                await RemoveImage();
                return;
            }

            var file = e.File;
            
            // Validate file type
            if (!IsValidImageType(file.ContentType))
            {
                Snackbar.Add("Please select a valid image file (JPG, JPEG, PNG).", Severity.Error);
                return;
            }

            // Validate file size
            if (file.Size > MaxFileSize)
            {
                Snackbar.Add($"File size exceeds the maximum limit of {GetFileSizeText(MaxFileSize)}.", Severity.Error);
                return;
            }

            _selectedFileName = file.Name;

            // Convert to base64
            using var stream = file.OpenReadStream(MaxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(bytes);

            UploadedImage = new FileUploadCommand
            {
                Name = file.Name,
                Data = base64String,
                Extension = file.ContentType,
                // Size = file.Size
            };

            await UploadedImageChanged.InvokeAsync(UploadedImage);
            StateHasChanged();

            Snackbar.Add("Image uploaded successfully!", Severity.Success);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error uploading image: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Validates if the provided content type is a supported image format.
    /// </summary>
    /// <param name="contentType">The MIME content type to validate.</param>
    /// <returns>True if the content type is a supported image format; otherwise, false.</returns>
    private static bool IsValidImageType(string contentType)
    {
        var supportedTypes = new[]
        {
            "image/jpeg",
            "image/jpg", 
            "image/png"
        };

        return supportedTypes.Contains(contentType.ToUpperInvariant());
    }

    /// <summary>
    /// Sets the drag hover CSS class when a file is dragged over the drop zone.
    /// </summary>
    private void SetDragClass()
    {
        _dragClass = "drag-hover";
    }

    /// <summary>
    /// Clears the drag hover CSS class when a file is no longer being dragged over the drop zone.
    /// </summary>
    private void ClearDragClass()
    {
        _dragClass = string.Empty;
    }

    /// <summary>
    /// Handles the drag enter event.
    /// </summary>
    private void OnDragEnter(DragEventArgs _)
    {
        SetDragClass();
    }

    /// <summary>
    /// Handles the drag leave event.
    /// </summary>
    private void OnDragLeave(DragEventArgs _)
    {
        ClearDragClass();
    }

    /// <summary>
    /// Handles the drag end event.
    /// </summary>
    private void OnDragEnd(DragEventArgs _)
    {
        ClearDragClass();
    }

    /// <summary>
    /// Handles the drop event.
    /// </summary>
    private void OnDrop(DragEventArgs _)
    {
        ClearDragClass();
    }

    #endregion
}
