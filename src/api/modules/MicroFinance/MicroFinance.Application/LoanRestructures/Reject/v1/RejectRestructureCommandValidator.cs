using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Reject.v1;

/// <summary>
/// Validator for RejectRestructureCommand.
/// </summary>
public sealed class RejectRestructureCommandValidator : AbstractValidator<RejectRestructureCommand>
{
    public RejectRestructureCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Restructure ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(LoanRestructure.MaxLengths.Notes)
            .WithMessage($"Rejection reason cannot exceed {LoanRestructure.MaxLengths.Notes} characters.");
    }
}
