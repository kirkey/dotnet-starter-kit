namespace FSH.Starter.Blazor.Client.Components;

/// <summary>
/// Reusable image uploader component with drag-and-drop functionality.
/// Supports image preview, validation, and upload to server via FileUploadCommand.
/// </summary>
public partial class ImageUploader : ComponentBase
{
    #region Injected Services

    /// <summary>
    /// Gets or sets the image URL service for constructing absolute image URLs using the API base URI.
    /// </summary>
    [Inject] private ImageUrlService ImageUrlService { get; set; } = null!;
    
    #endregion

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

    /// <summary>
    /// Unique id for the native InputFile element to avoid collisions when multiple components are on the page.
    /// </summary>
    private readonly string _nativeInputId = "iuNative_" + DefaultIdType.NewGuid().ToString("N");

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the image preview URL to display.
    /// Returns the uploaded image data URL if available, otherwise constructs the absolute URL
    /// using the API base URI (localhost:7000) via ImageUrlService.
    /// </summary>
    /// <returns>The image URL to display in the preview.</returns>
#pragma warning disable CA1055 // URI-like return values should not be strings
    public string? GetImagePreviewUrl()
#pragma warning restore CA1055
    {
        // If we have a newly uploaded image with base64 data, return it as a data URL
        if (!string.IsNullOrWhiteSpace(UploadedImage?.Data))
        {
            return $"data:{UploadedImage.Extension};base64,{UploadedImage.Data}";
        }

        // If we have a current image URL from the server, use ImageUrlService to construct the absolute URL
        // This ensures images are fetched from the API server (localhost:7000) not the Blazor client (localhost:7100)
        if (!string.IsNullOrWhiteSpace(CurrentImageUrl))
        {
            // If BaseUri parameter is provided, use it; otherwise delegate to ImageUrlService
            if (!string.IsNullOrWhiteSpace(BaseUri))
            {
                var result = CurrentImageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) 
                    ? CurrentImageUrl 
                    : $"{BaseUri.TrimEnd('/')}/{CurrentImageUrl.TrimStart('/')}";
                return result;
            }
            
            // Use ImageUrlService which uses the configured API base URL (localhost:7000)
            return ImageUrlService.GetAbsoluteUrl(CurrentImageUrl);
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
    /// Tries MudFileUpload first, then falls back to clicking the native input via JS.
    /// </summary>
    public async Task OpenFilePicker()
    {
        try
        {
            if (_fileUpload != null)
            {
                await _fileUpload.OpenFilePickerAsync();
                return;
            }

            // Fallback: programmatically click the native input with the unique id
            var js = $"(function(){{var el=document.getElementById('{_nativeInputId}'); if(el){{el.click();}}}})()";
            await Js.InvokeVoidAsync("eval", js);
        }
        catch
        {
            // try JS fallback if MudFileUpload fails
            try
            {
                var js = $"(function(){{var el=document.getElementById('{_nativeInputId}'); if(el){{el.click();}}}})()";
                await Js.InvokeVoidAsync("eval", js);
            }
            catch
            {
                // ignore silently — user will still be able to use the transparent native input
            }
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
    /// Handles the file selection change event from the native InputFile control.
    /// Delegates to the shared file processor so both native and MudFileUpload follow the same logic.
    /// </summary>
    /// <param name="e">The input file change event arguments.</param>
    private async Task OnFilesChanged(InputFileChangeEventArgs e)
    {
        if (e == null || e.FileCount == 0)
        {
            await RemoveImage();
            return;
        }

        await ProcessBrowserFileAsync(e.File);
    }

    /// <summary>
    /// Shared file processing logic for a single IBrowserFile.
    /// Performs size check, reads bytes, validates content and signature, converts to base64 and emits UploadedImage.
    /// </summary>
    /// <param name="file">The browser file to process.</param>
    private async Task ProcessBrowserFileAsync(IBrowserFile file)
    {
        if (file == null)
            return;

        // Validate file size first
        if (file.Size > MaxFileSize)
        {
            Snackbar.Add($"File size exceeds the maximum limit of {GetFileSizeText(MaxFileSize)}.", Severity.Error);
            return;
        }

        _selectedFileName = file.Name;

        // Read file into memory (we already enforced MaxFileSize when opening stream)
        using var stream = file.OpenReadStream(MaxFileSize);
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        // Validate file type: accept if content-type indicates image OR extension matches OR magic-bytes check
        var contentOk = !string.IsNullOrWhiteSpace(file.ContentType) && file.ContentType.Split(';')[0].Trim().StartsWith("image/", StringComparison.OrdinalIgnoreCase);
        var ext = Path.GetExtension(file.Name).TrimStart('.');
        var extOk = !string.IsNullOrWhiteSpace(ext) && (ext.Equals("jpg", StringComparison.OrdinalIgnoreCase) || ext.Equals("jpeg", StringComparison.OrdinalIgnoreCase) || ext.Equals("png", StringComparison.OrdinalIgnoreCase));
        var bytesOk = IsImageBytes(bytes);

        if (!contentOk && !extOk && !bytesOk)
        {
            // Build diagnostic info to help debug why a valid image might be rejected
            var sig = bytes.Length > 0 ? BitConverter.ToString(bytes.Take(Math.Min(8, bytes.Length)).ToArray()).Replace("-", " ") : "<empty>";
            var ct = file.ContentType ?? "<null>";
            var message = $"Invalid image. contentType='{ct}', ext='{ext}', bytes={bytes.Length}, signature='{sig}'";
            Snackbar.Add(message, Severity.Error);
            return;
        }

        // Convert to base64
        var base64String = Convert.ToBase64String(bytes);

        string MapContentTypeToExtension(string? contentType, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                contentType = contentType.Split(';')[0].Trim().ToLowerInvariant();
                return contentType switch
                {
                    "image/jpeg" => ".jpg",
                    "image/jpg" => ".jpg",
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    _ => string.Empty
                };
            }
            return Path.GetExtension(fileName);
        }

        var dotExt = MapContentTypeToExtension(file.ContentType, file.Name);
        if (string.IsNullOrWhiteSpace(dotExt))
        {
            dotExt = Path.GetExtension(file.Name);
        }
        if (!string.IsNullOrWhiteSpace(dotExt) && !dotExt.StartsWith('.'))
        {
            dotExt = "." + dotExt.TrimStart('.');
        }

        UploadedImage = new FileUploadCommand
        {
            Name = file.Name,
            Data = base64String,
            Extension = dotExt,
            Size = file.Size
        };

        await UploadedImageChanged.InvokeAsync(UploadedImage);
        StateHasChanged();

        Snackbar.Add("Image uploaded successfully!", Severity.Success);
    }

    /// <summary>
    /// Checks common image file signatures (magic bytes) to determine file type.
    /// Supports JPEG and PNG signatures.
    /// </summary>
    /// <param name="bytes">The file bytes to inspect.</param>
    /// <returns>True if the bytes match a supported image signature.</returns>
    private static bool IsImageBytes(byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
            return false;

        // JPEG: FF D8 FF
        if (bytes is [0xFF, 0xD8, 0xFF, ..])
            return true;

        // PNG: 89 50 4E 47 0D 0A 1A 0A
        if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
            && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A)
            return true;

        return false;
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

    /// <summary>
    /// Trigger a programmatic click on the native input element. Bound to the label's @onclick.
    /// </summary>
    /// <param name="e">Mouse event args (not used).</param>
    private async Task TriggerNativeInputClick(MouseEventArgs e)
    {
        try
        {
            // Use a short JS snippet to click the native input. This is executed in response to a user interaction.
            var js = $"(function(){{var el=document.getElementById('{_nativeInputId}'); if(el){{el.click();}}}})()";
            await Js.InvokeVoidAsync("eval", js);
        }
        catch
        {
            // ignore failures silently; the native transparent input should still receive clicks
        }
    }

    #endregion

}
