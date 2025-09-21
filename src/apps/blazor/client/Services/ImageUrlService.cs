// C#

namespace FSH.Starter.Blazor.Client.Services;

/// <summary>
/// Service that composes absolute image URLs by combining the configured API base Uri
/// with the relative image path stored in the backend (e.g. "/files/images/Category/alcohol.jpg").
/// </summary>
public sealed class ImageUrlService
{
    private readonly Uri _apiBase;

    /// <summary>
    /// Initializes a new instance of <see cref="ImageUrlService"/>.
    /// </summary>
    /// <param name="apiBase">Configured API base Uri (e.g. https://localhost:7000/).</param>
    public ImageUrlService(Uri apiBase)
    {
        _apiBase = apiBase ?? throw new ArgumentNullException(nameof(apiBase));
    }

    /// <summary>
    /// Returns an absolute URL for an image given the stored imageUrl (relative, e.g. /files/images/Category/alcohol.jpg).
    /// If imageUrl is already absolute, it is returned unchanged.
    /// </summary>
    /// <param name="imageUrl">Relative or absolute image path from backend.</param>
    /// <returns>Absolute URL string or empty string when input is null/empty.</returns>
    public string GetAbsoluteUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return string.Empty;

        if (Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            return imageUrl!;

        var rel = imageUrl!.StartsWith("/") ? imageUrl : "/" + imageUrl;
        var full = new Uri(_apiBase, rel);
        return full.ToString();
    }
}
