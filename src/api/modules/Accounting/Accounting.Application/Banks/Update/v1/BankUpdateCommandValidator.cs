using Accounting.Application.Banks.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Update.v1;

/// <summary>
/// Validator for the BankUpdateCommand with comprehensive validation rules.
/// Ensures data integrity and business rule compliance before bank update.
/// </summary>
public class BankUpdateCommandValidator : AbstractValidator<BankUpdateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankUpdateCommandValidator"/> class.
    /// </summary>
    /// <param name="repository">Repository for bank data access to check for duplicates.</param>
    public BankUpdateCommandValidator(
        [FromKeyedServices("accounting:banks")] IReadRepository<Bank> repository)
    {
        // Id validation
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Bank ID is required.");

        // BankCode validation
        RuleFor(x => x.BankCode)
            .NotEmpty().WithMessage("Bank code is required.")
            .MaximumLength(16).WithMessage("Bank code must not exceed 16 characters.")
            .Matches(@"^[A-Z0-9\-_]+$").WithMessage("Bank code must contain only uppercase letters, numbers, hyphens, and underscores.")
            .MustAsync(async (command, bankCode, cancellationToken) =>
            {
                var bank = await repository.FirstOrDefaultAsync(new BankByCodeSpec(bankCode), cancellationToken).ConfigureAwait(false);
                return bank == null || bank.Id == command.Id;
            }).WithMessage("Bank code already exists. Please use a unique code.");

        // Name validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Bank name is required.")
            .MaximumLength(256).WithMessage("Bank name must not exceed 256 characters.")
            .MinimumLength(2).WithMessage("Bank name must be at least 2 characters long.");

        // RoutingNumber validation (9 digits for US banks)
        RuleFor(x => x.RoutingNumber)
            .Matches(@"^\d{9}$").WithMessage("Routing number must be exactly 9 digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.RoutingNumber))
            .MustAsync(async (command, routingNumber, cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(routingNumber)) return true;
                var bank = await repository.FirstOrDefaultAsync(new BankByRoutingNumberSpec(routingNumber), cancellationToken).ConfigureAwait(false);
                return bank == null || bank.Id == command.Id;
            }).WithMessage("Routing number already exists. Each routing number must be unique.");

        // SwiftCode validation (8 or 11 characters)
        RuleFor(x => x.SwiftCode)
            .Matches(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$").WithMessage("SWIFT code must be 8 or 11 characters (format: 6 letters + 2 alphanumeric + optional 3 alphanumeric).")
            .When(x => !string.IsNullOrWhiteSpace(x.SwiftCode));

        // Address validation
        RuleFor(x => x.Address)
            .MaximumLength(512).WithMessage("Address must not exceed 512 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        // ContactPerson validation
        RuleFor(x => x.ContactPerson)
            .MaximumLength(128).WithMessage("Contact person name must not exceed 128 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactPerson));

        // PhoneNumber validation
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[\d\s\-\+\(\)]+$").WithMessage("Phone number contains invalid characters.")
            .MaximumLength(32).WithMessage("Phone number must not exceed 32 characters.")
            .MinimumLength(10).WithMessage("Phone number must be at least 10 characters long.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        // Email validation
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email address is not valid.")
            .MaximumLength(128).WithMessage("Email address must not exceed 128 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        // Website validation
        RuleFor(x => x.Website)
            .Must(BeAValidUrl).WithMessage("Website URL is not valid.")
            .MaximumLength(256).WithMessage("Website URL must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));

        // Description validation
        RuleFor(x => x.Description)
            .MaximumLength(1024).WithMessage("Description must not exceed 1024 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        // Notes validation
        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }

    /// <summary>
    /// Validates if a string is a valid URL.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

