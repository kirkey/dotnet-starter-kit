using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using FSH.Framework.Core.Origin;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Framework.Core.Storage.File.Features;
using FSH.Framework.Infrastructure.Common.Extensions;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Storage.Files;

/// <summary>
/// Local file storage service that uploads files to the server's local filesystem.
/// Returns a relative Uri (starting with '/') that represents the stored file path
/// so the application stores only the path portion in the database.
/// </summary>
public class LocalFileStorageService(IOptions<OriginOptions> originSettings) : IStorageService
{
    /// <summary>
    /// Uploads a file for entity type <typeparamref name="T"/> and returns a relative URI.
    /// Accepts raw base64 or data URLs (data:image/*;base64,...).
    /// Enforces a single, normalized extension and a sanitized filename.
    /// The returned Uri will be relative (e.g. "/files/images/Category/alcohol.jpg").
    /// </summary>
    /// <typeparam name="T">Entity type to create a folder for</typeparam>
    /// <param name="request">File upload request</param>
    /// <param name="supportedFileType">Allowed file types</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Relative Uri to the saved file</returns>
    public async Task<Uri> UploadAsync<T>(FileUploadCommand? request, FileType supportedFileType, CancellationToken cancellationToken = default)
        where T : class
    {
        if (request?.Data == null)
            throw new InvalidOperationException("No file data provided.");

        // Normalize extension: allow ".png", "png" or "image/png" and convert to ".png"
        static string NormalizeExtension(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var ext = input.Trim().ToLower(System.Globalization.CultureInfo.CurrentCulture);

            if (ext.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                ext = "." + ext.Split('/')[1];
            }
            else if (!ext.StartsWith('.'))
            {
                ext = "." + ext.TrimStart('.');
            }

            return ext;
        }

        var normalizedExt = NormalizeExtension(request.Extension);
        if (!supportedFileType.GetDescriptionList().Contains(normalizedExt))
            throw new InvalidOperationException("File format not supported.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new InvalidOperationException("Name is required.");

        // Accept both data URLs and raw base64 payloads
        string base64Data;
        if (request.Data.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
        {
            var match = Regex.Match(request.Data, "data:image/(?<type>.+?),(?<data>.+)");
            base64Data = match.Success ? match.Groups["data"].Value : string.Empty;
        }
        else
        {
            base64Data = request.Data;
        }

        if (string.IsNullOrWhiteSpace(base64Data))
            throw new InvalidOperationException("Invalid image data provided.");

        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
        if (streamData.Length <= 0)
            throw new InvalidOperationException("Uploaded file is empty.");

        // Ensure a safe folder per entity type
        string folder = typeof(T).Name;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            folder = folder.Replace(@"\", "/", StringComparison.Ordinal);
        }

        string folderName = supportedFileType switch
        {
            FileType.Image => Path.Combine("files", "images", folder),
            _ => Path.Combine("files", "others", folder),
        };

        string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        Directory.CreateDirectory(pathToSave);

        // Sanitize and enforce single extension (remove any extension included in request.Name)
        string rawName = Path.GetFileName(request.Name.Trim('"')); // strip any path parts or quotes
        string baseName = Path.GetFileNameWithoutExtension(rawName);
        baseName = RemoveSpecialCharacters(baseName).ReplaceWhitespace("-");

        if (string.IsNullOrWhiteSpace(baseName))
            throw new InvalidOperationException("A valid file name is required.");

        string fileName = baseName + normalizedExt; // append exactly once

        string fullPath = Path.Combine(pathToSave, fileName);
        string dbPath = Path.Combine(folderName, fileName);

        // Ensure unique filename by operating on the full filesystem path and then recomputing dbPath.
        if (File.Exists(fullPath))
        {
            string uniqueFullPath = NextAvailableFilename(fullPath);
            fullPath = uniqueFullPath;
            var uniqueFileName = Path.GetFileName(uniqueFullPath);
            dbPath = Path.Combine(folderName, uniqueFileName);
        }

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await streamData.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);

        // Ensure forward slashes and a leading slash for the relative path stored in DB
        var relativePath = "/" + dbPath.Replace("\\", "/").TrimStart('/');
        return new Uri(relativePath, UriKind.Relative);
    }

    /// <summary>
    /// Removes all characters except letters, digits, underscore and dot.
    /// </summary>
    public static string RemoveSpecialCharacters(string str) =>
        Regex.Replace(str, "[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);

    /// <summary>
    /// Attempts to delete a file at the given relative URI.
    /// Accepts either an absolute URI or a relative one. Relative URIs are resolved
    /// against the application's current directory to locate the physical file.
    /// </summary>
    /// <param name="path">Uri to the file (relative or absolute)</param>
    public void Remove(Uri? path)
    {
        if (path is null)
            return;

        string physicalPath;
        if (path.IsAbsoluteUri)
        {
            // If absolute, use AbsolutePath portion to resolve local file (strip leading '/')
            var absPath = path.AbsolutePath.TrimStart('/');
            physicalPath = Path.Combine(Directory.GetCurrentDirectory(), absPath);
        }
        else
        {
            var rel = path.ToString().TrimStart('/');
            physicalPath = Path.Combine(Directory.GetCurrentDirectory(), rel);
        }

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }

    private const string NumberPattern = "-{0}";

    private static string NextAvailableFilename(string path)
    {
        if (!File.Exists(path))
            return path;

        if (Path.HasExtension(path))
            return GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern));

        return GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        string tmp = string.Format(pattern, 1);
        if (!File.Exists(tmp))
            return tmp;

        int min = 1, max = 2;
        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
                min = pivot;
            else
                max = pivot;
        }

        return string.Format(pattern, max);
    }
}
