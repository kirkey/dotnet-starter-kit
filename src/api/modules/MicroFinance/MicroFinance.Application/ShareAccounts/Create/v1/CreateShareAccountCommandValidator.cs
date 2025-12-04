using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Create.v1;

public sealed class CreateShareAccountCommandValidator : AbstractValidator<CreateShareAccountCommand>
{
    public CreateShareAccountCommandValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty()
            .MaximumLength(ShareAccount.AccountNumberMaxLength)
            .WithMessage($"Account number must not exceed {ShareAccount.AccountNumberMaxLength} characters.");

        RuleFor(x => x.MemberId)
            .NotEmpty()
            .WithMessage("Member ID is required.");

        RuleFor(x => x.ShareProductId)
            .NotEmpty()
            .WithMessage("Share product ID is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(ShareAccount.NotesMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes must not exceed {ShareAccount.NotesMaxLength} characters.");
    }
}
