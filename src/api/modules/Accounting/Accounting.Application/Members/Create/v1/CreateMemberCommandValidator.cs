namespace Accounting.Application.Members.Create.v1;

/// <summary>
/// Validator for CreateMemberCommand.
/// </summary>
public sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.MemberNumber)
            .NotEmpty()
            .WithMessage("Member number is required.")
            .MaximumLength(64)
            .WithMessage("Member number cannot exceed 50 characters.");

        RuleFor(x => x.MemberName)
            .NotEmpty()
            .WithMessage("Member name is required.")
            .MaximumLength(256)
            .WithMessage("Member name cannot exceed 200 characters.");

        RuleFor(x => x.ServiceAddress)
            .NotEmpty()
            .WithMessage("Service address is required.")
            .MaximumLength(512)
            .WithMessage("Service address cannot exceed 500 characters.");

        RuleFor(x => x.MembershipDate)
            .NotEmpty()
            .WithMessage("Membership date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Membership date cannot be in the future.");

        RuleFor(x => x.AccountStatus)
            .Must(status => new[] { "Active", "Inactive", "Past Due", "Suspended", "Closed" }
                .Contains(status, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid account status. Must be: Active, Inactive, Past Due, Suspended, or Closed.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("Invalid email address format.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(32)
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone number cannot exceed 20 characters.");
    }
}

