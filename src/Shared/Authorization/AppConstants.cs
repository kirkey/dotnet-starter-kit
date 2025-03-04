using System.Collections.ObjectModel;

namespace Shared.Authorization;
public static class AppConstants
{
    public static readonly Collection<string> SupportedImageFormats =
    [
        ".jpeg",
        ".jpg",
        ".png"
    ];

    public const string StandardImageFormat = "image/jpeg";
    public const int MaxImageWidth = 1500;
    public const int MaxImageHeight = 1500;
    public const long MaxAllowedSize = 1000000; // Allows Max File Size of 1 Mb.
}
