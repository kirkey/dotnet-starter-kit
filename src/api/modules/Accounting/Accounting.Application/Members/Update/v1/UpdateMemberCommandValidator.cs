namespace Accounting.Application.Members.Update.v1;

/// <summary>
/// Validator for UpdateMemberCommand.
/// </summary>
public sealed class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.MemberName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrWhiteSpace(x.MemberName))
            .WithMessage("Member name cannot exceed 200 characters.");

        RuleFor(x => x.ServiceAddress)
            .MaximumLength(512)
            .When(x => !string.IsNullOrWhiteSpace(x.ServiceAddress))
            .WithMessage("Service address cannot exceed 500 characters.");

        RuleFor(x => x.AccountStatus)
            .Must(status => new[] { "Active", "Inactive", "Past Due", "Suspended", "Closed" }
                .Contains(status!, StringComparer.OrdinalIgnoreCase))
            .When(x => !string.IsNullOrWhiteSpace(x.AccountStatus))
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

