namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Validator for ImportGroceryItemsCommand with strict checks on file and type.
/// </summary>
public sealed class ImportGroceryItemsCommandValidator : AbstractValidator<ImportGroceryItemsCommand>
{
    private static readonly HashSet<string> AllowedExtensions = [".xls", ".xlsx"]; // enforce .xlsx only as per requirement

    public ImportGroceryItemsCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(f => f.IsValid()).WithMessage("File payload is invalid.");

        RuleFor(x => x.File.Extension)
            .NotEmpty()
            .Must(ext => AllowedExtensions.Contains(ext.Trim().ToLowerInvariant()))
            .WithMessage($"Only {string.Join(", ", AllowedExtensions)} files are supported.");

        RuleFor(x => x.File.Data)
            .NotEmpty().WithMessage("File data (base64) is required.")
            .Must(IsBase64).WithMessage("File data must be a valid base64 string.");

        RuleFor(x => x.File.Size)
            .LessThanOrEqualTo(10 * 1024 * 1024) // 10 MB safety limit
            .When(x => x.File.Size.HasValue)
            .WithMessage("File size exceeds maximum allowed (10 MB).");
    }

    private static bool IsBase64(string base64)
    {
        Span<byte> buffer = stackalloc byte[base64.Length];
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}

